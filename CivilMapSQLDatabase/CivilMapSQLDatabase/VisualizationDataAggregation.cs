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

        // Return points in a given area, given a specific center point and radius
        public List<PurifiedAddressModel> SelectCivilMapPurifiedAddress(double longitude, double latitude, double radius)
        {
            string commandText = "select * from CivilMapPurifiedAddress where " +
                                 "Longitude between @minLon and @maxLon " +
                                 "and Latitude between @minLat and @maxLat";

            double earthRadius = 6371; // in km
            var list = new List<PurifiedAddressModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    var maxLat = latitude + (radius / earthRadius);
                    var minLat = latitude - (radius / earthRadius);
                    var maxLon = longitude + Math.Asin(radius / earthRadius) / Math.Cos(DegreeToRadians(latitude));
                    var minLon = longitude - Math.Asin(radius / earthRadius) / Math.Cos(DegreeToRadians(latitude));

                    Debug.WriteLine("min / max lon: " + minLon + " " + maxLon);
                    Debug.WriteLine("min / max lat: " + minLat + " " + maxLat);

                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
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
                    }
                    connection.Close();

                    Debug.WriteLine("list length = " + list.Count());

                    foreach(var item in list)
                    {
                        Debug.WriteLine("Selected lon / lat: " + item.Longitude + " " + item.Latitude);
                    }
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
    }
}
