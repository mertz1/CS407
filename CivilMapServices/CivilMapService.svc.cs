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

namespace CivilMapServices
{
   
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CivilMapService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CivilMapService.svc or CivilMapService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CivilMapService : ICivilMapService
    {
        string connString = ConfigurationManager.ConnectionStrings["DBCS_TEST"].ConnectionString;
        string stored = @"F:\ValidateUser.txt";
        /* User Authentication */
        public string Login(UserData user)
        {          
            string response = "0";
            if (user.token != null && user.token != "")
            {
                TokenValidationStatus result = validateToken(user.token, user.username);
                using (StreamWriter sw = File.AppendText(stored))
                {
                    sw.WriteLine("-------------------Token Validate-----------------------");
                    sw.WriteLine("result1: {0}, result2:{1}", result.WrongUser, result.Expired);
                }
                if (result.WrongUser && result.Expired)
                {
                    response = "2";
                }
                else if (result.Expired)
                {
                    response = "3";
                }
                else if (result.WrongUser)
                {
                    response = "4";
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
                            response = "-1";
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
                            //byte[] key = Convert.FromBase64String(encodedKey);

                            using (StreamWriter sw = File.AppendText(stored))
                            {
                                sw.WriteLine("-------------------Validate User-----------------------");
                                sw.WriteLine("password:{0}", user.password);
                                sw.WriteLine("encoded-password:{0}", Convert.ToBase64String(encodedpassword));
                                sw.WriteLine("salt:{0}", encodedSalt);
                                sw.WriteLine("hashedPassword:{0}", Convert.ToBase64String(hashBytes));
                                sw.WriteLine("Stored Key:{0}", encodedKey);
                                sw.WriteLine("");
                                sw.WriteLine("Compare Keys Result: {0}", (Convert.ToBase64String(hashBytes) == encodedKey));
                            }
                            if (encodedKey == Convert.ToBase64String(hashBytes)) 
                            {
                                /* Validated Crendtial */
                                response = createToken(user.username, user.password);
                                using (StreamWriter sw = File.AppendText(stored))
                                {
                                    sw.WriteLine("-------------------Token Created-----------------------");
                                    sw.WriteLine("token:{0}", response);
                                }
                            }
                            else
                            {
                                /* Invalid Credential */
                                response = "1";
                            }
                        }
                    }
                }
                catch (Exception e) {
                    using (StreamWriter sw = File.AppendText(stored))
                    {
                        sw.WriteLine("ERROR: {0}", e);
                    }
                }
            }
            return response;
        }

        /* For testing purpose, create test users and encoded password 
    * For password encoding, we using SHA-256 as hash function then concat
    with random salt to ensure maximum security
*/
        public void CreateUser(string username, string password)
        {
            //testing
            string stored = @"F:\ValidateUser.txt";

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
                    using (StreamWriter sw = File.AppendText(stored))
                    {
                        sw.WriteLine("-------------------Create User-----------------------");
                        sw.WriteLine("password:{0}", password);
                        sw.WriteLine("encoded-password:{0}", Convert.ToBase64String(encodedpassword));
                        sw.WriteLine("salt: {0}", salt);
                        sw.WriteLine("hashedPassword:{0}", hashedPassword);
                    }
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
            using (StreamWriter sw = File.AppendText(stored))
            {
                sw.WriteLine("-------------------Subfunction: Token Validate-----------------------");
                sw.WriteLine("cached token:{0}", token);
                sw.WriteLine("time: {0}", Encoding.UTF8.GetString(time));
                sw.WriteLine("key: {0}", Encoding.UTF8.GetString(key));
                sw.WriteLine("id: {0}", Encoding.UTF8.GetString(Id));
                sw.WriteLine("-------------------Time Test-----------------------");
                sw.WriteLine("now time: {0} | new time {1}", when, DateTime.UtcNow.AddHours(-1));
                sw.WriteLine("Time Expired Test: {0}", when < DateTime.UtcNow.AddHours(-2));
            }
            return result;
        }
    }
}
