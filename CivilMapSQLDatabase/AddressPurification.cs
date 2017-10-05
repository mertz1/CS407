﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CivilMapSQLDatabase
{
    public class AddressPurification : CivilMapSQLDatabaseConnection
    {
        public string AddCivilMapPurifiedAddress(PurifiedAddressModel model)
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

                    commandText = "Insert into CivilMapPurifiedAddress (PurifiedAddressId, StreetNumber, Street, City, Zipcode, Longitude, Latitude) values " +
                                  "(@PurifiedAddressId, @StreetNumber, @Street, @City, @Zipcode, @Longitude, @Latitude)";
                    command = new SqlCommand(commandText, connection);

                    Guid purifiedAddressId = Guid.NewGuid();
                    command.Parameters.AddWithValue("@PurifiedAddressId", purifiedAddressId);
                    command.Parameters.AddWithValue("@StreetNumber", model.StreetNumber);
                    command.Parameters.AddWithValue("@Street", model.Street);
                    command.Parameters.AddWithValue("@City", model.City);
                    command.Parameters.AddWithValue("@Zipcode", model.Zipcode);
                    command.Parameters.AddWithValue("@Longitude", model.Longitude);
                    command.Parameters.AddWithValue("@Latitude", model.Latitude);
                    command.ExecuteNonQuery();
                    connection.Close();
                    
                    return purifiedAddressId.ToString();
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
                                 "StreetNumber = @StreetNumber and Street like @Street and City like @City and Zipcode = @Zipcode";
            var list = new List<PurifiedAddressModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@StreetNumber", model.StreetNumber);
                    command.Parameters.AddWithValue("@Street", ("%" + model.Street + "%").ToString());
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
            string commandText = "select * from CivilMapNonPurifiedAddress where StreetNumber = @StreetNumber and Street like @Street and City like @City and Zipcode = @Zipcode ";
            var list = new List<NonPurifiedAddressModel>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@StreetNumber", model.StreetNumber);
                    command.Parameters.AddWithValue("@Street", ("%" + model.Street + "%").ToString());
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
                                 "update CivilMapNonPurifiedAddress set StreetNumber=@StreetNumber, Street=@Street, City=@City, Zipcode=@Zipcode " +
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

        public string AddCivilMapPoints(PointsModel item)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapPoints')" +
                                 "create table [dbo].[CivilMapPoints] (" +
                                 "[PointId] UNIQUEIDENTIFIER NOT NULL," +
                                 "[PurifiedAddressId] UNIQUEIDENTIFIER NULL," +
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

                    Guid pointId = Guid.NewGuid();
                    command.Parameters.AddWithValue("@PointId", pointId);
                    command.Parameters.AddWithValue("@PurifiedAddressId", item.PurifiedAddressId ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                    connection.Close();

                    return pointId.ToString();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                    System.Diagnostics.Debug.WriteLine("Hits here!");
                }
                return null;
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
                            PointId = reader.GetGuid(0),
                            PurifiedAddressId = reader.IsDBNull(1) ? (Guid?)null : reader.GetGuid(1)
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
                            PointId = reader.GetGuid(0),
                            PurifiedAddressId = reader.IsDBNull(1) ? (Guid?)null : reader.GetGuid(1)
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