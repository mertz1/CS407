using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CivilMapSQLDatabase
{
    public class AddressPurification : CivilMapSQLDatabaseConnection
    {

        public object AddCivilMapPurifiedAddress(PurifiedAddressModel model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapPurifiedAddress')" +
                                 "CREATE TABLE[dbo].[CivilMapPurifiedAddress](" +
                                 "[PurifiedAddressId] UNIQUEIDENTIFIER NOT NULL," +
                                 "[StreetNumber] NVARCHAR(384) NULL," +
                                 "[Street] NVARCHAR(384) NULL," +
                                 "[City] NVARCHAR(384) NULL," +
                                 "[Zipcode] NVARCHAR(384) NULL," +
                                 "[Longitude] DECIMAL(9, 6) NOT NULL," +
                                 "[Latitude] DECIMAL(9, 6) NOT NULL," +
                                 "PRIMARY KEY CLUSTERED([PurifiedAddressId] ASC))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                    commandText = "Insert into CivilMapPurifiedAddress (StreetNumber, Street, City, Zipcode, Longitude, Latitude) output INSERTED.PurifiedAddressId values " +
                                  "(@StreetNumber, @Street, @City, @Zipcode, @Longitude, @Latitude)";
                    command = new SqlCommand(commandText, connection);

                    command.Parameters.AddWithValue("@StreetNumber", model.StreetNumber);
                    command.Parameters.AddWithValue("@Street", model.Street);
                    command.Parameters.AddWithValue("@City", model.City);
                    command.Parameters.AddWithValue("@Zipcode", model.Zipcode);
                    command.Parameters.AddWithValue("@Longitude", model.Longitude);
                    command.Parameters.AddWithValue("@Latitude", model.Latitude);
                    object purifiedAddressId = command.ExecuteScalar();
                    connection.Close();

                    return purifiedAddressId;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return null;
            }
        }

        public List<PurifiedAddressModel> GetCivilMapPurifiedAddress()
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "select * from CivilMapPurifiedAddress";
            var list = new List<PurifiedAddressModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new PurifiedAddressModel
                        {
                            PurifiedAddressId = reader.GetGuid(0),
                            StreetNumber = reader.GetString(1),
                            Street = reader.GetString(2),
                            City = reader.GetString(3),
                            Zipcode = reader.GetString(4),
                            Longitude = reader.GetDecimal(5),
                            Latitude = reader.GetDecimal(6)
                        });
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public List<PurifiedAddressModel> SelectCivilMapPurifiedAddress(PurifiedAddressModel model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "select * from CivilMapPurifiedAddress where " +
                                 "StreetNumber = @StreetNumber and Street = @Street and City like @City and Zipcode = @Zipcode";
            var list = new List<PurifiedAddressModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@StreetNumber", model.StreetNumber);
                    command.Parameters.AddWithValue("@Street", (model.Street).ToString());
                    command.Parameters.AddWithValue("@City", ("%" + model.City + "%").ToString());
                    command.Parameters.AddWithValue("@Zipcode", model.Zipcode);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new PurifiedAddressModel
                        {
                            PurifiedAddressId = reader.GetGuid(0),
                            StreetNumber = reader.IsDBNull(1)? null: reader.GetString(1),
                            Street = reader.IsDBNull(2) ? null : reader.GetString(2),
                            City = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Zipcode = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Longitude = reader.GetDecimal(5),
                            Latitude = reader.GetDecimal(6)
                        });
                    }
                    connection.Close();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public string AddCivilMapNonPurifiedAddress(NonPurifiedAddressModel item)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapNonPurifiedAddress')" +
                                 "CREATE TABLE[dbo].[CivilMapNonPurifiedAddress](" +
                                 "[NonPurifiedAddressId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY," +
                                 "[StreetNumber] NVARCHAR(384) NULL," +
                                 "[Street] NVARCHAR(384) NULL," +
                                 "[City] NVARCHAR(384) NULL," +
                                 "[Zipcode] NVARCHAR(384) NULL," +
                                 "[PurifiedAddressId] UNIQUEIDENTIFIER NULL," + 
                                 "CONSTRAINT[FK_CivilMapNonPurifiedAddress_CivilMapPurifiedAddress] FOREIGN KEY([PurifiedAddressId]) REFERENCES[dbo].[CivilMapPurifiedAddress] ([PurifiedAddressId]))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                    commandText = "Insert into CivilMapNonPurifiedAddress (NonPurifiedAddressId, StreetNumber, Street, City, Zipcode, PurifiedAddressId) values " +
                                  "(@NonPurifiedAddressId, @StreetNumber, @Street, @City, @Zipcode, @PurifiedAddressId)";
                    command = new SqlCommand(commandText, connection);

                    Guid nonPurifiedAddressId = Guid.NewGuid();
                    command.Parameters.AddWithValue("@NonPurifiedAddressId", nonPurifiedAddressId);
                    command.Parameters.AddWithValue("@StreetNumber", item.StreetNumber);
                    command.Parameters.AddWithValue("@Street", item.Street);
                    command.Parameters.AddWithValue("@City", item.City);
                    command.Parameters.AddWithValue("@Zipcode", item.Zipcode);
                    command.Parameters.AddWithValue("@PurifiedAddressId", item.PurifiedAddressId?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                    connection.Close();

                    return nonPurifiedAddressId.ToString();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return null;
;            }
        }

        public object AddValidationCivilMapNonPurifiedAddress(NonPurifiedAddressModel item)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapNonPurifiedAddress')" +
                                 "CREATE TABLE[dbo].[CivilMapNonPurifiedAddress](" +
                                 "[NonPurifiedAddressId] NUMERIC(18) NOT NULL," +
                                 "[StreetNumber] NVARCHAR(384) NULL," +
                                 "[Street] NVARCHAR(384) NULL," +
                                 "[City] NVARCHAR(384) NULL," +
                                 "[Zipcode] NVARCHAR(384) NULL," +
                                 "[PurifiedAddressId] UNIQUEIDENTIFIER NOT NULL," +
                                 "PRIMARY KEY CLUSTERED([NonPurifiedAddressId] ASC)," +
                                 "CONSTRAINT[FK_CivilMapNonPurifiedAddress_CivilMapPurifiedAddress] FOREIGN KEY([PurifiedAddressId]) REFERENCES[dbo].[CivilMapPurifiedAddress] ([PurifiedAddressId]))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                    object insertedId;

                    if (item.City == null)
                    {
                        item.City = "";
                    }
                    if (item.Zipcode == null)
                    {
                        item.Zipcode = "";
                    }

                    commandText = 
                                  "Insert into CivilMapNonPurifiedAddress (StreetNumber, Street, City, Zipcode) output INSERTED.NonPurifiedAddressId values " +
                                  "(@StreetNumber, @Street, @City, @Zipcode)";
                    command = new SqlCommand(commandText, connection);

                    command.Parameters.AddWithValue("@StreetNumber", item.StreetNumber);
                    command.Parameters.AddWithValue("@Street", item.Street);
                    command.Parameters.AddWithValue("@City", item.City);
                    command.Parameters.AddWithValue("@Zipcode", item.Zipcode);
                    insertedId = command.ExecuteScalar();
                    connection.Close();
                    return insertedId;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                    return null;
                }
            }
        }

        public List<NonPurifiedAddressModel> GetCivilMapNonPurifiedAddress()
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "select * from CivilMapNonPurifiedAddress";
            var list = new List<NonPurifiedAddressModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new NonPurifiedAddressModel
                        {
                            NonPurifiedAddressId = reader.GetGuid(0),
                            StreetNumber = reader.IsDBNull(1)? null:reader.GetString(1),
                            Street = reader.IsDBNull(2) ? null : reader.GetString(2),
                            City = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Zipcode = reader.IsDBNull(4) ? null : reader.GetString(4),
                            PurifiedAddressId = reader.IsDBNull(5) ? (Guid?)null: reader.GetGuid(5)
                        });
                    }
                    connection.Close();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public List<NonPurifiedAddressModel> SelectCivilMapNonPurifiedAddress(NonPurifiedAddressModel model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "select * from CivilMapNonPurifiedAddress where StreetNumber = @StreetNumber and Street = @Street and City like @City and Zipcode = @Zipcode ";
            var list = new List<NonPurifiedAddressModel>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@StreetNumber", model.StreetNumber);
                    command.Parameters.AddWithValue("@Street", (model.Street).ToString());
                    command.Parameters.AddWithValue("@City", ("%" + model.City + "%").ToString());
                    command.Parameters.AddWithValue("@Zipcode", model.Zipcode);

                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        list.Add(new NonPurifiedAddressModel
                        {
                            NonPurifiedAddressId = reader.GetGuid(0),
                            StreetNumber = reader.IsDBNull(1)? null:reader.GetString(1),
                            Street = reader.IsDBNull(2) ? null : reader.GetString(2),
                            City = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Zipcode = reader.IsDBNull(4) ? null : reader.GetString(4),
                            PurifiedAddressId = reader.IsDBNull(5) ? (Guid?)null : reader.GetGuid(5)
                        });
                    }
                    connection.Close();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public void UpdateCivilMapNonPurifiedAddress(NonPurifiedAddressModel model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if exists (select * from CivilMapNonPurifiedAddress where NonPurifiedAddressId=@NonPurifiedAddressId) " +
                                 "update CivilMapNonPurifiedAddress set PurifiedAddressId = @PurifiedAddressId " +
                                 "where NonPurifiedAddressId=@NonPurifiedAddressId " + 
                                 "else " +
                                 "Insert into CivilMapNonPurifiedAddress (NonPurifiedAddressId, StreetNumber, Street, City, Zipcode, PurifiedAddressId) values " +
                                 "(@NonPurifiedAddressId, @StreetNumber, @Street, @City, @Zipcode, @PurifiedAddressId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@NonPurifiedAddressId", model.NonPurifiedAddressId);
                    command.Parameters.AddWithValue("@StreetNumber", model.StreetNumber);
                    command.Parameters.AddWithValue("@Street", model.Street);
                    command.Parameters.AddWithValue("@City", model.City);
                    command.Parameters.AddWithValue("@Zipcode", model.Zipcode);
                    command.Parameters.AddWithValue("@PurifiedAddressId", model.PurifiedAddressId);
                    command.ExecuteNonQuery();

                    connection.Close();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public async Task<Guid?> ValidateAddress(PurifiedAddressModel model)
        {
            var purifiedResults = new List<PurifiedAddressModel>();
            purifiedResults = SelectCivilMapPurifiedAddress(model);

            Debug.WriteLine(purifiedResults.Count);

            if(purifiedResults.Count == 0)
            { 
                Console.WriteLine("Address Did Not Exist");
                NonPurifiedAddressModel nonPurifiedModel = new NonPurifiedAddressModel();
                nonPurifiedModel.Street = model.Street;
                nonPurifiedModel.StreetNumber = model.StreetNumber;
                nonPurifiedModel.Zipcode = model.Zipcode;
                nonPurifiedModel.City = model.City;

                var nonPurifiedResult = new List<NonPurifiedAddressModel>();
                nonPurifiedResult = SelectCivilMapNonPurifiedAddress(nonPurifiedModel);

                Debug.WriteLine("NonPurified List: " + nonPurifiedResult);
                
                Debug.WriteLine("Street:" + nonPurifiedModel.Street.ToString().Replace(" ", "+"));

                if (nonPurifiedResult.Count == 0 )
                {
                    object nonPurifiedResponse;
                    nonPurifiedResponse = AddValidationCivilMapNonPurifiedAddress(nonPurifiedModel);
                    Debug.WriteLine("Added Address with ID: " + nonPurifiedResponse);

                    //call geocod.io
                    //string apiCall = "street=" + nonPurifiedModel.StreetNumber.ToString();
                    //apiCall += "+" + nonPurifiedModel.Street.ToString().Replace(" ", "+") + "&";

                    //if(nonPurifiedModel.City != null)
                    //{
                    //    apiCall += "city=" + nonPurifiedModel.City.ToString() + "&";
                    //}
                    //if(nonPurifiedModel.Zipcode != null)
                    //{
                    //    apiCall += "zip=" + nonPurifiedModel.Zipcode.ToString() + "&";
                    //}
                    //apiCall += "state=IL&";

                    //apiCall += "api_key=3551a95da75a999c89a259153a77d2aa9a3d5a2";

                    //using (var client = new HttpClient())
                    //{
                    //    client.BaseAddress = new Uri("https://api.geocod.io/v1/geocode?");
                    //    client.DefaultRequestHeaders.Accept.Clear();
                    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //    HttpResponseMessage resposne = client.GetAsync(apiCall);
                    //}

                    GeocodioClient.GeocodioClient geoClient = new GeocodioClient.GeocodioClient("3551a95da75a999c89a259153a77d2aa9a3d5a2");

                    string street = nonPurifiedModel.StreetNumber.ToString() + " " + nonPurifiedModel.Street.ToString();

                    PurifiedAddressModel validatedAddress = geoClient.Geocode(street, null, null, nonPurifiedModel.Zipcode.ToString());


                }
                else if(nonPurifiedResult.Count == 1 && nonPurifiedResult[0].PurifiedAddressId == null)
                {
                    Debug.WriteLine("Calling Third Party API");

                    GeocodioClient.GeocodioClient geoClient = new GeocodioClient.GeocodioClient("3551a95da75a999c89a259153a77d2aa9a3d5a2");

                    string street = nonPurifiedModel.StreetNumber.ToString() + " " + nonPurifiedModel.Street.ToString();

                    Debug.WriteLine(nonPurifiedModel.City);

                    PurifiedAddressModel validatedAddress = geoClient.Geocode(street, null, null, nonPurifiedModel.Zipcode.ToString());

                    object newPurifiedId = AddCivilMapPurifiedAddress(validatedAddress);

                    NonPurifiedAddressModel updateModel = new NonPurifiedAddressModel();

                    updateModel.NonPurifiedAddressId = nonPurifiedResult[0].NonPurifiedAddressId;
                    updateModel.PurifiedAddressId = Guid.Parse(newPurifiedId.ToString());

                    UpdateCivilMapNonPurifiedAddress(updateModel);


                }

            }

            Console.WriteLine("Address Exists: " + purifiedResults);

            return null;
        }


        public void AddCivilMapPoints(PointsModel item)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapPoints')" +
                                 "create table [dbo].[CivilMapPoints] (" +
                                 "[PointId] NUMERIC (18) NOT NULL," +
                                 "[PurifiedAddressId] UNIQUEIDENTIFIER NOT NULL," +
                                 "PRIMARY KEY CLUSTERED ([PointId] ASC)," +
                                 "CONSTRAINT [FK_CivilMapPoints_CivilMapPurifiedAddress] FOREIGN KEY ([PurifiedAddressId]) REFERENCES [dbo].[CivilMapPurifiedAddress] ([PurifiedAddressId]))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                    commandText = "if not exists (select * from CivilMapPoints where PointId = @PointId) " + 
                                  "Insert into CivilMapPoints (PointId, PurifiedAddressId) values " +
                                  "(@PointId, @PurifiedAddressId)";
                    command = new SqlCommand(commandText, connection);

                    command.Parameters.AddWithValue("@PointId", item.PointId);
                    command.Parameters.AddWithValue("@PurifiedAddressId", item.PurifiedAddressId);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public List<PointsModel> GetCivilMapPoints()
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "select * from CivilMapPoints";
            var list = new List<PointsModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new PointsModel
                        {
                            PointId = reader.GetDecimal(0),
                            PurifiedAddressId = reader.GetGuid(1)
                        });
                    }
                    connection.Close();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public List<PointsModel> SelectCivilMapPoints(PointsModel model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "select * from CivilMapPoints where PointId=@PointId";
            var list = new List<PointsModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@PointId", model.PointId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new PointsModel
                        {
                            PointId = reader.GetDecimal(0),
                            PurifiedAddressId = reader.GetGuid(1)
                        });
                    }
                    connection.Close();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public void DeleteTable()
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "drop table [dbo].[CivilMapNonPurifiedAddress]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}