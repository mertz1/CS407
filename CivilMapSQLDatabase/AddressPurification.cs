using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CivilMapSQLDatabase
{
    public class AddressPurification : CivilMapSQLDatabaseConnection
    {
        //public void AddKristinTest(List<PurifiedAddressModel> list)
        //{
        //    string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
        //    string commandText = "if exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='KristinTest')" +
        //                         "drop table [dbo].[KristinTest]" +
        //                         "create table [dbo].[KristinTest] (" +
        //                         "[PurifiedAddressId] NUMERIC(18) NOT NULL," +
        //                         "[PurifiedAddress] NVARCHAR(MAX) NULL," +
        //                         "[Longitude] DECIMAL(9, 6) NOT NULL," +
        //                         "[Latitude] DECIMAL(9, 6) NOT NULL," +
        //                         "PRIMARY KEY CLUSTERED([PurifiedAddressId] ASC))";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            SqlCommand command = new SqlCommand(commandText, connection);
        //            connection.Open();
        //            command.ExecuteNonQuery();

        //            foreach (var item in list)
        //            {
        //                commandText = "Insert into KristinTest (PurifiedAddressId, PurifiedAddress, Longitude, Latitude) values " +
        //                "(@PurifiedAddressId, @PurifiedAddress, @Longitude, @Latitude)";
        //                command = new SqlCommand(commandText, connection);
                        
        //                command.Parameters.AddWithValue("@PurifiedAddressId", item.PurifiedAddressId);
        //                command.Parameters.AddWithValue("@PurifiedAddress", item.PurifiedAddress);
        //                command.Parameters.AddWithValue("@Longitude", item.Longitude);
        //                command.Parameters.AddWithValue("@Latitude", item.Latitude);
        //                command.ExecuteNonQuery();
        //            }
        //            connection.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
        //        }
        //    }
        //}

        public Guid? AddCivilMapPurifiedAddress(PurifiedAddressModel model)
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
                Guid purifiedAddressId = Guid.NewGuid();
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    bool allNull = false;

                    commandText1 = "if not exists (select * from CivilMapPurifiedAddress where PurifiedAddress like @Address and Longitude = @Longitude and Latitude = @Latitude)" +
                        "Insert into CivilMapPurifiedAddress (PurifiedAddressId, StreetNumber, Street, City, Zipcode, Longitude, Latitude) values " +
                        "(@PurifiedAddressId, @StreetNumber, @Street, @City, @Zipcode, @Longitude, @Latitude)";

                    if ((model.StreetNumber == null || model.StreetNumber.Length == 0) && (model.Street == null || model.Street.Length == 0) && (model.City == null || model.City.Length == 0) && (model.Zipcode == null || model.Zipcode.Length == 0))
                    {
                        commandText = "if not exists(select * from CivilMapPurifiedAddress) ";
                        allNull = true;
                    }
                    else
                    {
                        commandText = "if not exists (select * from CivilMapPurifiedAddress where ";
                        allNull = false;
                    }
                    if (model.StreetNumber != null && model.StreetNumber.Length != 0)
                        commandText += "StreetNumber = @StreetNumber ";
                    if (model.Street != null && model.Street.Length != 0)
                        commandText += "and Street like @Street";
                    if (model.City != null && model.City.Length != 0)
                        commandText += "";

                    command = new SqlCommand(commandText, connection);

                    command.Parameters.AddWithValue("@Address", ("%" + model.PurifiedAddress + "%").ToString());
                    command.Parameters.AddWithValue("@PurifiedAddressId", purifiedAddressId);
                    command.Parameters.AddWithValue("@PurifiedAddress", model.PurifiedAddress);
                    command.Parameters.AddWithValue("@Longitude", model.Longitude);
                    command.Parameters.AddWithValue("@Latitude", model.Latitude);
                    command.ExecuteNonQuery();
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
                            PurifiedAddress = new AddressModel
                            {
                                StreetNumber = reader.GetString(0),
                                Street = reader.GetString(1),
                                City = reader.GetString(2),
                                Zipcode = reader.GetString(3)
                            },
                            Longitude = reader.GetDecimal(2),
                            Latitude = reader.GetDecimal(3)
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
            string commandText = "select * from CivilMapPurifiedAddress where PurifiedAddressId=@PurifiedAddressId";
            var list = new List<PurifiedAddressModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@PurifiedAddressId", model.PurifiedAddressId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new PurifiedAddressModel
                        {
                            PurifiedAddressId = reader.GetGuid(0),
                            PurifiedAddress = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Longitude = reader.GetDecimal(2),
                            Latitude = reader.GetDecimal(3)
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

        public void AddCivilMapNonPurifiedAddress(NonPurifiedAddressModel item)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapNonPurifiedAddress')" +
                                 "CREATE TABLE[dbo].[CivilMapNonPurifiedAddress](" + 
                                 "[NonPurifiedAddressId] NUMERIC(18) NOT NULL," + 
                                 "[NonPurifiedAddress] NVARCHAR(MAX) NULL," +
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

                    commandText = "if not exists (select * from CivilMapNonPurifiedAddress where NonPurifiedAddressId = @NonPurifiedAddressId)" + 
                                  "Insert into CivilMapNonPurifiedAddress (NonPurifiedAddressId, NonPurifiedAddress, PurifiedAddressId) values " +
                                  "(@NonPurifiedAddressId, @NonPurifiedAddress, @PurifiedAddressId)";
                    command = new SqlCommand(commandText, connection);

                    command.Parameters.AddWithValue("@NonPurifiedAddressId", item.NonPurifiedAddressId);
                    command.Parameters.AddWithValue("@NonPurifiedAddress", item.NonPurifiedAddress);
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
                            NonPurifiedAddressId = reader.GetDecimal(0),
                            NonPurifiedAddress = reader.IsDBNull(1)? null:reader.GetString(1),
                            PurifiedAddressId = reader.GetGuid(2)
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
            string commandText = "select * from CivilMapNonPurifiedAddress where NonPurifiedAddress like @NonPurifiedAddress";
            var list = new List<NonPurifiedAddressModel>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@NonPurifiedAddress", ("%" + model.NonPurifiedAddress + "%").ToString());
                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        list.Add(new NonPurifiedAddressModel
                        {
                            NonPurifiedAddressId = reader.GetDecimal(0),
                            NonPurifiedAddress = reader.IsDBNull(1)? null:reader.GetString(1),
                            PurifiedAddressId = reader.GetGuid(2)
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
                                 "update CivilMapNonPurifiedAddress set NonPurifiedAddress=@NonPurifiedAddress, PurifiedAddressId=@PurifiedAddressId " +
                                 "where NonPurifiedAddressId=@NonPurifiedAddressId " + 
                                 "else " +
                                 "Insert into CivilMapNonPurifiedAddress (NonPurifiedAddressId, NonPurifiedAddress, PurifiedAddressId) values " +
                                 "(@NonPurifiedAddressId, @NonPurifiedAddress, @PurifiedAddressId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@NonPurifiedAddressId", model.NonPurifiedAddressId);
                    command.Parameters.AddWithValue("@NonPurifiedAddress", model.NonPurifiedAddress);
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
            string commandText = "drop table [dbo].[CivilMapPurifiedCrime]";

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