using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CivilMapSQLDatabase
{
    public class VisualizationDataAggregation : CivilMapSQLDatabaseConnection
    {
        string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";

        // Return points in a selectd circle area, given a specific center point and radius
        public List<PurifiedAddressModel> SelectCivilMapPurifiedAddress(double lon, double lat, double radius)
        {
            string commandText = "Select PurifiedAddressId, StreetNumber, Street, City, Zipcode, Longitude, Latitude, " +
                                 "acos(sin(@lat)*sin(radians(Latitude)) + cos(@lat)*cos(radians(Latitude))*cos(radians(Longitude)-@lon)) * @earthRadius As D " +
                                 "From ( " +
                                    "Select * " +
                                    "From CivilMapPurifiedAddress where " +
                                    "Longitude between @minLon and @maxLon " +
                                    "and Latitude between @minLat and @maxLat " +
                                 ") As FirstCut " +
                                 "Where (acos(sin(@lat)*sin(radians(Latitude)) + cos(@lat)*cos(radians(Latitude))*cos(radians(Longitude)-@lon)) * @earthRadius) <= @radius " +
                                 "Order By D";

            double earthRadius = 6371; // in km
            var list = new List<PurifiedAddressModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    var maxLat = lat + (radius / earthRadius);
                    var minLat = lat - (radius / earthRadius);
                    var maxLon = lon + Math.Asin(radius / earthRadius) / Math.Cos(DegreeToRadians(lat));
                    var minLon = lon - Math.Asin(radius / earthRadius) / Math.Cos(DegreeToRadians(lat));

                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@lat", Convert.ToDecimal(DegreeToRadians(lat)));
                    command.Parameters.AddWithValue("@lon", Convert.ToDecimal(DegreeToRadians(lon)));
                    command.Parameters.AddWithValue("@earthRadius", Convert.ToDecimal(earthRadius));
                    command.Parameters.AddWithValue("@radius", Convert.ToDecimal(radius));
                    command.Parameters.AddWithValue("@minLat", Convert.ToDecimal(minLat));
                    command.Parameters.AddWithValue("@maxLat", Convert.ToDecimal(maxLat));
                    command.Parameters.AddWithValue("@minLon", Convert.ToDecimal(minLon));
                    command.Parameters.AddWithValue("@maxLon", Convert.ToDecimal(maxLon));
                    
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new PurifiedAddressModel
                        {
                            PurifiedAddressId = reader.GetGuid(0),
                            AddressModel = new AddressModel
                            {
                                StreetNumber = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Street = reader.IsDBNull(2) ? null : reader.GetString(2),
                                City = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Zipcode = reader.IsDBNull(4) ? null : reader.GetString(4)
                            },
                            Longitude = reader.GetDecimal(5),
                            Latitude = reader.GetDecimal(6)
                        });
                        Debug.WriteLine(reader.GetDouble(7));
                    }
                    connection.Close();

                    //Debug.WriteLine("list length = " + list.Count());
                    //Debug.WriteLine("min / max lon: " + minLon + " " + maxLon);
                    //Debug.WriteLine("min / max lat: " + minLat + " " + maxLat);

                    //foreach (var item in list)
                    //{
                    //    Debug.WriteLine("Selected lon / lat: " + item.Longitude + " " + item.Latitude);
                    //}
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public double DegreeToRadians(double degree)
        {
            double radians = Math.PI * degree / 180.0;
            return radians;
        }

        public void InsertIntoCpdBeats(List<string> objectId, List<string> district, List<string> sector, List<string> beat, List<string> beat_num)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    for (int i = 0; i < objectId.Count(); i++)
                    {
                        string commandText = "Insert into Cpd_Beats (ObjectId, District, Sector, Beat, Beat_Num) values " +
                                 "(@ObjectId, @District, @Sector, @Beat, @Beat_Num)";
                        SqlCommand command = new SqlCommand(commandText, connection);

                        command.Parameters.AddWithValue("@ObjectId", objectId[i]);
                        command.Parameters.AddWithValue("@District", district[i]);
                        command.Parameters.AddWithValue("@Sector", sector[i]);
                        command.Parameters.AddWithValue("@Beat", beat[i]);
                        command.Parameters.AddWithValue("@Beat_Num", beat_num[i]);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    Debug.WriteLine("finished!");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
