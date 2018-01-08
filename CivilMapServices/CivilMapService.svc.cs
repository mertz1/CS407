using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel.Activation;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;

namespace CivilMapServices
{
   
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CivilMapService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CivilMapService.svc or CivilMapService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CivilMapService : ICivilMapService
    {
        /*----------------------------------Global Variable-----------------------------------*/
        string connString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        string stored = @"D:\StoredCusMap.txt";

        /*----------------------------------Authentication-----------------------------------*/
        /* User Authentication */
        public string Login(UserData user)
        {          
            string response = "0";
            if (user.token != null && user.token != "")
            {
                TokenValidationStatus result = validateToken(user.token, user.username);
                /*using (StreamWriter sw = File.AppendText(stored))
                {
                    sw.WriteLine("-------------------Token Validate-----------------------");
                    sw.WriteLine("result1: {0}, result2:{1}", result.WrongUser, result.Expired);
                }*/
                if (result.WrongUser && result.Expired)
                {
                    response = "Error: Invalid User & Section has been time out";
                }
                else if (result.Expired)
                {
                    response = "Error: Section has been time out";
                }
                else if (result.WrongUser)
                {
                    response = "Error: Invalid User";
                }
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                        string checkuser = "select count(*) from UserData where UserName='" + user.username + "'";
                        SqlCommand com = new SqlCommand(checkuser, con);
                        con.Open();
                        int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                        if (temp != 1)
                        {
                            /* If user does not exist, return error code -1 */
                            response = "Error: User Does Not Exist.";
                        }
                        else
                        {
                            /* Get stored hashkey and salt */
                            string storedPassword = "select Password from UserData where UserName='" + user.username + "'";
                            string storedSalt = "select Salt from UserData where UserName='" + user.username + "'";
                            SqlCommand getPassword = new SqlCommand(storedPassword, con);
                            SqlCommand getSalt = new SqlCommand(storedSalt, con);
                            string encodedKey = getPassword.ExecuteScalar().ToString();
                            string encodedSalt = getSalt.ExecuteScalar().ToString();

                            /* Generate hashkey and salt with the password, then compare with the stored data */
                            byte[] encodedpassword = Encoding.UTF8.GetBytes(user.password + encodedSalt);
                            byte[] hashBytes =
                            new System.Security.Cryptography.SHA256Cng().ComputeHash(encodedpassword);

                      
                            /* Stored Password(hashed) */
                            if (encodedKey == Convert.ToBase64String(hashBytes)) 
                            {
                                /* Validated Crendtial */
                                response = createToken(user.username, user.password);
                            }
                            else
                            {
                                /* Invalid Credential */
                                response = "Error: Invalid Username/Password";
                            }
                        }
                    }
                }
                catch (Exception e) {
                }
            }
            return response;
        }

        /* For testing purpose, create test users and encoded password 
        For password encoding, we using SHA-256 as hash function then concat
        with random salt to ensure maximum security*/
        public void CreateUser(string username, string password)
        {
            string salt = "";
            string hashedPassword = "";
            //random salt

                salt = Guid.NewGuid().ToString();
                byte[] random_salt = Encoding.UTF8.GetBytes(salt);
                salt = Convert.ToBase64String(random_salt);
                byte[] encodedpassword = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes =
                new System.Security.Cryptography.SHA256Cng().ComputeHash(encodedpassword);
                hashedPassword = Convert.ToBase64String(hashBytes);

                
          
            /* Insert username, password and user's salt into database */
            try {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string checkuser = "IF EXISTS(Select * from UserData where Username='" + username + "') Update UserData SET Password='" 
                        + hashedPassword + "', salt='" + salt + "' WHERE Username='" + username + "' ELSE INSERT INTO UserData([username], [password], [salt]) VALUES('" 
                        + username + "', '" + hashedPassword + "', '" + salt
                        + "')";
                    SqlCommand com = new SqlCommand(checkuser, con);
                    con.Open();
                    com.ExecuteNonQuery();
                }
            }catch (Exception e){}
            return;
        }

        /* Token generator with time stamp and userid */
        protected string createToken(string username, string password)
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            byte[] Id = Encoding.UTF8.GetBytes(username);
            byte[] data = new byte[time.Length + key.Length + Id.Length];

            System.Buffer.BlockCopy(time, 0, data, 0, time.Length);
            System.Buffer.BlockCopy(key, 0, data, time.Length, key.Length);
            System.Buffer.BlockCopy(Id, 0, data, time.Length + key.Length, Id.Length);

            return Convert.ToBase64String(data.ToArray());
        }

        /* Take the generated token string and validate it */
        public TokenValidationStatus validateToken(string token, string username)
        {
          
            var result = new TokenValidationStatus();
            byte[] data = Convert.FromBase64String(token);
            byte[] time = data.Take(8).ToArray();
            byte[] key = data.Skip(8).Take(16).ToArray();
            byte[] Id = data.Skip(24).ToArray();

            /* Expired Time Stamp */
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(time, 0));
            if (when < DateTime.UtcNow.AddHours(-1))
            {
                result.Expired = true;
            }
            /* Incorrect User */
            if (username != Encoding.UTF8.GetString(Id))
            {
                result.WrongUser = true;
            }
            return result;
        }




        /*-----------------------------------Point Request/Reponse----------------------------------*/
        /* Return data for map render according to different request */
        public PointSet RequestPoints(RequestPoint Request) {
            PointSet PointSet = new PointSet();
            PointSet.error = "0";
            /*using (StreamWriter sw = File.AppendText(stored))
                {
                    sw.WriteLine("-------------------Get Reqeust-----------------------");
                    sw.WriteLine("type:{0}", Request.type);
                }*/
            try {
                if (Request.type == "arrest" || Request.type == "crime") {

                    if (Request.beat != null && Request.beat != "") {
                        PointSet = GetPointsByBeat(Request.type, Request.beat);

                    }else if (Request.date_range != null && Request.date_range != "" && Int32.Parse(Request.date_range) != 0)
                    {
                        int date_range = Int32.Parse(Request.date_range);
                        if (date_range > 0 || date_range <= 31)
                            PointSet = GetPointByPredefinedRange(date_range, Request.type);

                    }else if (Request.date_start != "" && Request.date_start != null
                            && Request.date_end != "" && Request.date_end != null){

                            string S_Date = (DateTime.ParseExact(Request.date_start, "yyyy-MM-dd",
                                               System.Globalization.CultureInfo.InvariantCulture)).ToString("yyyy-MM-dd");
                            string E_Date = (DateTime.ParseExact(Request.date_end, "yyyy-MM-dd",
                                               System.Globalization.CultureInfo.InvariantCulture)).ToString("yyyy-MM-dd");
                            PointSet = GetPointByDates(S_Date, E_Date, Request.type);

                    }else{
                            PointSet = GetPointsByType(Request.type);
                    }
                }
                else { 
                    /* Invalid Type */
                    PointSet.error = "Invalid Filter Type";
                    return PointSet;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return PointSet;
        }

        /* Return data for map render according to different request */
        public PointSet RequestPointsByRadius(RequestPointByRadius RequestbyRadius) {
            PointSet pointset = new PointSet();
            PointSet result_set = new PointSet();
            RequestPoint prerequst = new RequestPoint();
            prerequst.type = RequestbyRadius.type;
            prerequst.date_start = RequestbyRadius.date_start;
            prerequst.date_end = RequestbyRadius.date_end;
            prerequst.date_range = RequestbyRadius.date_range;
            const double CONVERTER = 0.000621371;

            try
            {
                pointset = RequestPoints(prerequst);
                int count = 0;
                /*Calculate distance*/
                foreach (Point p in pointset.Points) {
                    double lon2, lat2;
                    double.TryParse(p.longitude, out lon2);
                    double.TryParse(p.latitude, out lat2);

                    if (double.IsNaN(lon2) || double.IsInfinity(lon2) || double.IsNaN(lat2) || double.IsInfinity(lat2))
                    {
                        continue;
                    }
                    var sCoord = new GeoCoordinate(RequestbyRadius.center_latitude, RequestbyRadius.center_longitude);
                    var eCoord = new GeoCoordinate(lat2, lon2);
                    p.distance = sCoord.GetDistanceTo(eCoord) * CONVERTER;
                    if (p.distance <= RequestbyRadius.radius) { 
                            result_set.Points.Add(p);
                            count++;
                    }
                }
                result_set.count = count;
            }
            catch (Exception e) {
                result_set.error = e.ToString();
                return result_set;
            }
            return result_set;
        }

        /*Get Data fron DB by predefined date range */
        protected PointSet GetPointByPredefinedRange(int range, string type) {
            PointSet PointSet = new PointSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("getPointsByDates", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    string today = DateTime.Now.ToString("yyyy-MM-dd");
                    
                    //for testing purpose
                    today = "2014-01-30";
                    DateTime temp_date = (DateTime.ParseExact("2014-01-30", "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture));
                    string start_date = temp_date.AddDays(range * -1).ToString("yyyy-MM-dd");

                    cmd.Parameters.AddWithValue("@startDate", start_date);
                    cmd.Parameters.AddWithValue("@endDate", today);
                    cmd.Parameters.AddWithValue("@type", type);
                    con.Open();

                    SqlDataReader Point_reader = cmd.ExecuteReader();
                    int count = 0;
                    while (Point_reader.Read())
                    {
                        count++;
                        Point point = new Point();
                        point.longitude = Point_reader["Longitude"].ToString();
                        point.latitude = Point_reader["Latitude"].ToString();
                        point.date = Point_reader["Date"].ToString();
                        PointSet.Points.Add(point);
                    }
                    PointSet.count = count;
                    Point_reader.Close();
                }

            }
            catch (Exception e)
            {
                PointSet.error = e.ToString();
                PointSet.count = -1;
            }
            return PointSet;
        }

        /*Get Data fron DB by customized date range*/
        protected PointSet GetPointByDates(string Start, string End, string type)
        {
            PointSet PointSet = new PointSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("getPointsByDates", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@startDate", Start);
                    cmd.Parameters.AddWithValue("@endDate", End);
                    cmd.Parameters.AddWithValue("@type", type);
                    con.Open();

                    SqlDataReader Point_reader = cmd.ExecuteReader();
                    int count = 0;
                    while (Point_reader.Read())
                    {
                        count++;
                        Point point = new Point();
                        point.longitude = Point_reader["Longitude"].ToString();
                        point.latitude = Point_reader["Latitude"].ToString();
                        point.date = Point_reader["Date"].ToString();
                        PointSet.Points.Add(point);
                    }
                    PointSet.count = count;
                    Point_reader.Close();
                }

            }
            catch (Exception e)
            {
                PointSet.error = e.ToString();
                PointSet.count = -1;
            }
            return PointSet;
        }

        /*Get Data fron DB by type: crime/arrest*/
        protected PointSet GetPointsByType(string type)
        {
            PointSet PointSet = new PointSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("getPointsByType", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", type);
                    con.Open();

                    SqlDataReader Point_reader = cmd.ExecuteReader();
                    int count = 0;
                    while (Point_reader.Read())
                    {
                        count++;
                        Point point = new Point();
                        point.longitude = Point_reader["Longitude"].ToString();
                        point.latitude = Point_reader["Latitude"].ToString();
                        point.date = Point_reader["Date"].ToString();
                        PointSet.Points.Add(point);
                    }
                    PointSet.count = count;
                    Point_reader.Close();
                }

            }
            catch (Exception e) {
                PointSet.error = e.ToString();
                PointSet.count = -1;
            }
            return PointSet;
        }

        /* Get Data from DB by type & beats */
        protected PointSet GetPointsByBeat(string type, string beat) {
            PointSet PointSet = new PointSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                
                    SqlCommand cmd = new SqlCommand("getPointsByBeat", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@beat", beat);
                    con.Open();

                    SqlDataReader Point_reader = cmd.ExecuteReader();
                    int count = 0;
                    while (Point_reader.Read())
                    {
                        count++;
                        Point point = new Point();
                        point.longitude = Point_reader["Longitude"].ToString();
                        point.latitude = Point_reader["Latitude"].ToString();
                        point.date = Point_reader["Date"].ToString();
                        PointSet.Points.Add(point);
                    }
                    PointSet.count = count;
                    Point_reader.Close();
                }

            }
            catch (Exception e)
            {
                PointSet.error = e.ToString();
                PointSet.count = -1;
            }
            return PointSet;
        }

        /*----------------------------------------Filter Data Request---------------------------------*/
        public List<string> getBeats(string type) {
            List<string> beats = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("getBeats", con);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader Beats_reader = cmd.ExecuteReader();
                    while (Beats_reader.Read())
                    {
                       
                        string beat = "";
                        beat = Beats_reader["Beat"].ToString();
                        beats.Add(beat);
                    }
                    Beats_reader.Close();
                }

            }
            catch (Exception e)
            {
                string beat = "error: " + e.ToString();
                beats.Add(beat);
            }
            return beats;
        }

        /*----------------------------------------Stored Uploaded Heatmap Points--------------------------------*/
        public int UploadHeatmapPoints(UploadedPoints CustomizedPoints) {
            CommaDelimitedStringCollection long_list = new CommaDelimitedStringCollection();
            CommaDelimitedStringCollection lat_list = new CommaDelimitedStringCollection();
            foreach (CustomizedPoint point in CustomizedPoints.points) {
                long_list.Add(point.longitude);
                lat_list.Add(point.latitude);
            }

            String longitudes = long_list.ToString();
            String latitudes = lat_list.ToString();
           
            int id = 0;
                try
                {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                        SqlCommand check_cmd = new SqlCommand("countCustomizedHeatMap", con);
                        SqlCommand stored_cmd = new SqlCommand("storeCustomizedPoints", con);

                        check_cmd.CommandType = CommandType.StoredProcedure;
                        stored_cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        //get count of existing stored heatmaps
                        id = (Int32)check_cmd.ExecuteScalar() + 1;

                        stored_cmd.Parameters.AddWithValue("@latitude", latitudes);
                        stored_cmd.Parameters.AddWithValue("@longitude", longitudes);
                        stored_cmd.Parameters.AddWithValue("@sectionid", id);
                        stored_cmd.ExecuteNonQuery();
                        return id;
                    }
                }
                catch (Exception e){
                 return -1;
                }
        }

        /*----------------------------------------Get Points from Section ID--------------------------------*/
        public PointSet GetPointsFromSectionId(int sectionId){

            PointSet points = new PointSet();
            if (sectionId >= 0)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connString))
                    {
                        SqlCommand cmd = new SqlCommand("getCustomizedHeatMap", con);
                        SqlCommand validate_cmd = new SqlCommand("validateSectionId", con);

                        validate_cmd.CommandType = CommandType.StoredProcedure; 
                        validate_cmd.Parameters.AddWithValue("@sectionId", sectionId);

                        con.Open();

                        int validate = (Int32)validate_cmd.ExecuteScalar();

                        if (validate > 0)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@sectionId", sectionId);

                            SqlDataReader Point_reader = cmd.ExecuteReader();
                            int count = 0;
                            while (Point_reader.Read())
                            {
                                count++;
                                Point point = new Point();
                                point.longitude = Point_reader["Longitude"].ToString();
                                point.latitude = Point_reader["Latitude"].ToString();
                                points.Points.Add(point);
                            }
                            points.count = count;
                            Point_reader.Close();
                            return points;
                        }
                        else {
                            con.Close();
                            points.error = "Section ID not found";
                            points.count = -1;
                            return points;
                        }
                        
                    }

                }
                catch (Exception e)
                {
                    points.error = e.ToString();
                    points.count = -1;
                    return points;
                }
            }else{
                points.error = "Invalid Section ID";
                points.count = -1;
                return points;
            }
        }

        /*----------------------------------------Get Points from past 7 days-------------------------------*/
        public PointSet GetPastWeekPoints(String type) {
            PointSet PointSet = new PointSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("getPointsByDates", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    string end_date = DateTime.Now.ToString("yyyy-MM-dd");
                    string start_date = DateTime.Now.AddDays(7 * -1).ToString("yyyy-MM-dd");

                    cmd.Parameters.AddWithValue("@startDate", start_date);
                    cmd.Parameters.AddWithValue("@endDate", end_date);
                    cmd.Parameters.AddWithValue("@type", type);
                    con.Open();

                    SqlDataReader Point_reader = cmd.ExecuteReader();
                    int count = 0;
                    while (Point_reader.Read())
                    {
                        count++;
                        Point point = new Point();
                        point.longitude = Point_reader["Longitude"].ToString();
                        point.latitude = Point_reader["Latitude"].ToString();
                        point.date = Point_reader["Date"].ToString();
                        PointSet.Points.Add(point);
                    }
                    PointSet.count = count;
                    Point_reader.Close();
                }

            }
            catch (Exception e)
            {
                PointSet.error = e.ToString();
                PointSet.count = -1;
            }
            return PointSet;
        }

        /*----------------------------------------Get Points for past year-------------------------------*/
        public PointSet GetPastYearPoints(String type)
        {
            PointSet PointSet = new PointSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("getPointsByDates", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    int year = (DateTime.Now.Year) - 1;
                    string start_date = new DateTime(year, 1, 1).ToString("yyyy-MM-dd");
                    string end_date = new DateTime(year, 12, 31).ToString("yyyy-MM-dd");

                    cmd.Parameters.AddWithValue("@startDate", start_date);
                    cmd.Parameters.AddWithValue("@endDate", end_date);
                    cmd.Parameters.AddWithValue("@type", type);
                    con.Open();

                    SqlDataReader Point_reader = cmd.ExecuteReader();
                    int count = 0;
                    while (Point_reader.Read())
                    {
                        count++;
                        Point point = new Point();
                        point.longitude = Point_reader["Longitude"].ToString();
                        point.latitude = Point_reader["Latitude"].ToString();
                        point.date = Point_reader["Date"].ToString();
                        PointSet.Points.Add(point);
                    }
                    PointSet.count = count;
                    Point_reader.Close();
                }

            }
            catch (Exception e)
            {
                PointSet.error = e.ToString();
                PointSet.count = -1;
            }
            return PointSet;
        }

        /*----------------------------------------Get Points for this year-------------------------------*/
        public PointSet GetThisYearPoints(String type)
        {
            PointSet PointSet = new PointSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("getPointsByDates", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    int year = DateTime.Now.Year;
                    string start_date = new DateTime(year, 1, 1).ToString("yyyy-MM-dd");
                    string end_date = new DateTime(year, 12, 31).ToString("yyyy-MM-dd");

                    cmd.Parameters.AddWithValue("@startDate", start_date);
                    cmd.Parameters.AddWithValue("@endDate", end_date);
                    cmd.Parameters.AddWithValue("@type", type);
                    con.Open();

                    SqlDataReader Point_reader = cmd.ExecuteReader();
                    int count = 0;
                    while (Point_reader.Read())
                    {
                        count++;
                        Point point = new Point();
                        point.longitude = Point_reader["Longitude"].ToString();
                        point.latitude = Point_reader["Latitude"].ToString();
                        point.date = Point_reader["Date"].ToString();
                        PointSet.Points.Add(point);
                    }
                    PointSet.count = count;
                    Point_reader.Close();
                }

            }
            catch (Exception e)
            {
                PointSet.error = e.ToString();
                PointSet.count = -1;
            }
            return PointSet;
        }

        /*------------------------------Get All Police Beats of Chicago Area----------------------*/
        public List<String> getCpdBeats()
        {
            List<String> beats = new List<String>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("getCpdBeats", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader Info_reader = cmd.ExecuteReader();
                    while (Info_reader.Read())
                    {
                        string beat = Info_reader.GetString(0);
                        beats.Add(beat);
                    }
                    Info_reader.Close();
                    return beats;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        /*------------------------------Get All Districts of Chicago Area----------------------*/
        public List<String> getCpdDistricts()
        {
            List<String> districts = new List<String>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("getCpdDistricts", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader Info_reader = cmd.ExecuteReader();
                    while (Info_reader.Read())
                    {
                        String district = Info_reader.GetString(0);
                        districts.Add(district);
                    }
                    Info_reader.Close();
                    return districts;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        /*------------------------------Get All Sectors of Chicago Area----------------------*/
        public List<String> getCpdSectors()
        {
            List<String> sectors = new List<String>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("getCpdSectors", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader Info_reader = cmd.ExecuteReader();
                    while (Info_reader.Read())
                    {
                        String sector = Info_reader.GetString(0);
                        sectors.Add(sector);
                    }
                    Info_reader.Close();
                    return sectors;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        /*------------------------------Get Name of All Area in Chicago----------------------*/
        public List<String> getCpdAreas()
        {
            List<String> area = new List<String>();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("getCpdAreas", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader Info_reader = cmd.ExecuteReader();
                    while (Info_reader.Read())
                    {
                        String a = Info_reader.GetString(0);
                        area.Add(a);
                    }
                    Info_reader.Close();
                    return area;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        /*------------------------------Get Points for Polygon----------------------*/
        public PointSet GetPointsForPolygon(Polygon polygon) {
            PointSet PointSet = new PointSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    if (polygon.type.ToUpper() == "CRIME")
                    {
                        cmd = new SqlCommand("getCrimesByCpdPolygons", con);
                    }
                    else if (polygon.type.ToUpper() == "ARREST")
                    {
                        cmd = new SqlCommand("getArrestsByCpdPolygons", con);
                        cmd.Parameters.AddWithValue("@sector", polygon.sector);
                    }
                    else {
                        PointSet.error = "Invalid type of event, please select crime or arrest";
                        PointSet.count = -1;
                        return PointSet;
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@polygon", polygon.option.ToLower());
                    cmd.Parameters.AddWithValue("@area", polygon.area);
                    cmd.Parameters.AddWithValue("@district", polygon.district);
                    cmd.Parameters.AddWithValue("@beat", polygon.beat);
                 
                    con.Open();

                    SqlDataReader Point_reader = cmd.ExecuteReader();
                    int count = 0;
                    while (Point_reader.Read())
                    {
                        count++;
                        Point point = new Point();
                        point.longitude = Point_reader.GetDecimal(0).ToString();
                        point.latitude = Point_reader.GetDecimal(1).ToString();
                        PointSet.Points.Add(point);
                    }
                    PointSet.count = count;
                    Point_reader.Close();
                }

            }
            catch (Exception e)
            {
                PointSet.error = e.ToString();
                PointSet.count = -1;
            }
            return PointSet;
        }
    }
}
