using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Globalization;

namespace CivilMapSQLDatabase
{
    public class AddressPurification : CivilMapSQLDatabaseConnection
    {
        string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
        
        public object AddCivilMapPurifiedAddress(PurifiedAddressModel model)
        {
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

                    command.Parameters.AddWithValue("@StreetNumber", model.AddressModel.StreetNumber);
                    command.Parameters.AddWithValue("@Street", model.AddressModel.Street);
                    command.Parameters.AddWithValue("@City", model.AddressModel.City);
                    command.Parameters.AddWithValue("@Zipcode", model.AddressModel.Zipcode);
                    command.Parameters.AddWithValue("@Longitude", model.Longitude);
                    command.Parameters.AddWithValue("@Latitude", model.Latitude);
                    object purifiedAddressId = command.ExecuteScalar();
                    connection.Close();

                    return purifiedAddressId;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return null;
            }
        }

        public List<PurifiedAddressModel> GetCivilMapPurifiedAddress()
        {
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
                            AddressModel = new AddressModel
                            {
                                StreetNumber = reader.GetString(1),
                                Street = reader.GetString(2),
                                City = reader.GetString(3),
                                Zipcode = reader.GetString(4)
                            },
                            Longitude = reader.GetDecimal(5),
                            Latitude = reader.GetDecimal(6)
                        });
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public List<PurifiedAddressModel> SelectCivilMapPurifiedAddressById(PurifiedAddressModel model)
        {
            string commandText = "select * from CivilMapPurifiedAddress where " +
                                 "PurifiedAddressId = @PurifiedAddressId";
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
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public List<PurifiedAddressModel> SelectCivilMapPurifiedAddress(PurifiedAddressModel model)
        {
            string commandText = "select * from CivilMapPurifiedAddress where " +
                                 "StreetNumber = @StreetNumber and Street = @Street and City like @City and Zipcode = @Zipcode";
            var list = new List<PurifiedAddressModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@StreetNumber", model.AddressModel.StreetNumber);
                    command.Parameters.AddWithValue("@Street", (model.AddressModel.Street).ToString());
                    command.Parameters.AddWithValue("@City", (model.AddressModel.City).ToString());
                    command.Parameters.AddWithValue("@Zipcode", model.AddressModel.Zipcode);
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
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public object AddCivilMapNonPurifiedAddress(NonPurifiedAddressModel item)
        {
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

                    if (item.AddressModel.City == null)
                    {
                        item.AddressModel.City = "";
                    }
                    if (item.AddressModel.Zipcode == null)
                    {
                        item.AddressModel.Zipcode = "";
                    }

                    commandText = 
                                  "Insert into CivilMapNonPurifiedAddress (StreetNumber, Street, City, Zipcode) output INSERTED.NonPurifiedAddressId values " +
                                  "(@StreetNumber, @Street, @City, @Zipcode)";
                    command = new SqlCommand(commandText, connection);

                    command.Parameters.AddWithValue("@StreetNumber", item.AddressModel.StreetNumber);
                    command.Parameters.AddWithValue("@Street", item.AddressModel.Street);
                    command.Parameters.AddWithValue("@City", item.AddressModel.City);
                    command.Parameters.AddWithValue("@Zipcode", item.AddressModel.Zipcode);
                    insertedId = command.ExecuteScalar();
                    connection.Close();
                    return insertedId;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    return null;
                }
            }
        }

        public List<NonPurifiedAddressModel> GetCivilMapNonPurifiedAddress()
        {
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
                            AddressModel = new AddressModel
                            {
                                StreetNumber = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Street = reader.IsDBNull(2) ? null : reader.GetString(2),
                                City = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Zipcode = reader.IsDBNull(4) ? null : reader.GetString(4),
                            },
                            PurifiedAddressId = reader.IsDBNull(5) ? (Guid?)null: reader.GetGuid(5)
                        });
                    }
                    connection.Close();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public List<NonPurifiedAddressModel> SelectAliasNonPurifiedAddress(Guid id)
        {
            string commandText = "select * from CivilMapNonPurifiedAddress where PurifiedAddressId = @PurifiedAddressId";
            var list = new List<NonPurifiedAddressModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@PurifiedAddressId", id);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new NonPurifiedAddressModel
                        {
                            NonPurifiedAddressId = reader.GetGuid(0),
                            AddressModel = new AddressModel
                            {
                                StreetNumber = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Street = reader.IsDBNull(2) ? null : reader.GetString(2),
                                City = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Zipcode = reader.IsDBNull(4) ? null : reader.GetString(4),
                            },
                            PurifiedAddressId = reader.IsDBNull(5) ? (Guid?)null : reader.GetGuid(5)
                        });
                    }
                    connection.Close();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public List<NonPurifiedAddressModel> SelectCivilMapNonPurifiedAddress(NonPurifiedAddressModel model)
        {
            string commandText = "select * from CivilMapNonPurifiedAddress where StreetNumber = @StreetNumber and Street = @Street and City like @City and Zipcode = @Zipcode ";
            var list = new List<NonPurifiedAddressModel>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@StreetNumber", model.AddressModel.StreetNumber);
                    command.Parameters.AddWithValue("@Street", (model.AddressModel.Street).ToString());
                    command.Parameters.AddWithValue("@City", (model.AddressModel.City).ToString());
                    command.Parameters.AddWithValue("@Zipcode", model.AddressModel.Zipcode);

                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        list.Add(new NonPurifiedAddressModel
                        {
                            NonPurifiedAddressId = reader.GetGuid(0),
                            AddressModel = new AddressModel
                            {
                                StreetNumber = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Street = reader.IsDBNull(2) ? null : reader.GetString(2),
                                City = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Zipcode = reader.IsDBNull(4) ? null : reader.GetString(4),
                            },
                            PurifiedAddressId = reader.IsDBNull(5) ? (Guid?)null : reader.GetGuid(5)
                        });
                    }
                    connection.Close();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public void UpdateCivilMapNonPurifiedAddress(NonPurifiedAddressModel model)
        {
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
                    command.Parameters.AddWithValue("@StreetNumber", model.AddressModel.StreetNumber);
                    command.Parameters.AddWithValue("@Street", model.AddressModel.Street);
                    command.Parameters.AddWithValue("@City", model.AddressModel.City);
                    command.Parameters.AddWithValue("@Zipcode", model.AddressModel.Zipcode);
                    command.Parameters.AddWithValue("@PurifiedAddressId", model.PurifiedAddressId);
                    command.ExecuteNonQuery();

                    connection.Close();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public async Task<Guid?> ValidateAddress(AddressModel model)
        {
            var purifiedAddress = new PurifiedAddressModel();
            purifiedAddress.AddressModel = model;

            var purifiedResults = new List<PurifiedAddressModel>();
            purifiedResults = SelectCivilMapPurifiedAddress(purifiedAddress);

            Debug.WriteLine(purifiedResults.Count);

            if(purifiedResults.Count == 0)
            { 
                Console.WriteLine("Address Did Not Exist");
                NonPurifiedAddressModel nonPurifiedModel = new NonPurifiedAddressModel();
                nonPurifiedModel.AddressModel = model;

                var nonPurifiedResult = new List<NonPurifiedAddressModel>();
                nonPurifiedResult = SelectCivilMapNonPurifiedAddress(nonPurifiedModel);

                Debug.WriteLine("NonPurified List: " + nonPurifiedResult);
                
                Debug.WriteLine("Street:" + nonPurifiedModel.AddressModel.Street.ToString().Replace(" ", "+"));

                if (nonPurifiedResult.Count == 0 )
                {
                    nonPurifiedModel.NonPurifiedAddressId = (Guid) AddCivilMapNonPurifiedAddress(nonPurifiedModel);

                    GeocodioClient.GeocodioClient geoClient = new GeocodioClient.GeocodioClient("3551a95da75a999c89a259153a77d2aa9a3d5a2");

                    string street = nonPurifiedModel.AddressModel.StreetNumber.ToString() + " " + nonPurifiedModel.AddressModel.Street.ToString();

                    PurifiedAddressModel validatedAddress = geoClient.Geocode(street, nonPurifiedModel.AddressModel.City, "IL", nonPurifiedModel.AddressModel.Zipcode.ToString());

                    object newPurifiedId = AddCivilMapPurifiedAddress(validatedAddress);    //Get Purified ID

                    nonPurifiedModel.PurifiedAddressId = Guid.Parse(newPurifiedId.ToString());

                    UpdateCivilMapNonPurifiedAddress(nonPurifiedModel);
                }
                else if(nonPurifiedResult.Count == 1 && nonPurifiedResult[0].PurifiedAddressId == null)
                {
                    Debug.WriteLine("Calling Third Party API");

                    GeocodioClient.GeocodioClient geoClient = new GeocodioClient.GeocodioClient("3551a95da75a999c89a259153a77d2aa9a3d5a2");

                    string street = nonPurifiedModel.AddressModel.StreetNumber.ToString() + " " + nonPurifiedModel.AddressModel.Street.ToString();

                    Debug.WriteLine(nonPurifiedModel.AddressModel.City);

                    PurifiedAddressModel validatedAddress = geoClient.Geocode(street, nonPurifiedModel.AddressModel.City, "IL", nonPurifiedModel.AddressModel.Zipcode.ToString());

                    object newPurifiedId = AddCivilMapPurifiedAddress(validatedAddress);

                    NonPurifiedAddressModel updateModel = new NonPurifiedAddressModel();

                    updateModel.NonPurifiedAddressId = nonPurifiedResult[0].NonPurifiedAddressId;
                    updateModel.PurifiedAddressId = Guid.Parse(newPurifiedId.ToString());

                    UpdateCivilMapNonPurifiedAddress(updateModel);


                }

            }
            return null;
        }

        public List<PurifiedAddressModel?> Validate100Addresses()
        {
            string commandText = "select ARREST_ID, STREET_NO, STREET_NME, CITY, ZIP_CD from CivilMapArrest ORDER BY ARREST_ID OFFSET 240 ROWS FETCH NEXT 100 ROWS ONLY";

            System.Type type;
            PurifiedAddressModel purifiedAddress = new PurifiedAddressModel();
            NonPurifiedAddressModel nonPurifiedAddress = new NonPurifiedAddressModel();
            List<String> arrests = new List<String>();
            List<AddressModel> list = new List<AddressModel>();
            List<PurifiedAddressModel> purifiedAddressList = new List<PurifiedAddressModel>();
            List<PurifiedAddressModel> purifiedSelectResult = new List<PurifiedAddressModel>();
            List<NonPurifiedAddressModel> nonPurifiedAddressList = new List<NonPurifiedAddressModel>();
            List<NonPurifiedAddressModel> nonPurifiedSelectResult = new List<NonPurifiedAddressModel>();
            List<String> addresses = new List<String>();
            string address;
            int fieldCount;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    //Debug.WriteLine(reader);

                    while (reader.Read())
                    {
                        fieldCount = reader.FieldCount;

                        arrests.Add(reader.GetString(0));
                        list.Add(new AddressModel
                        {
                            StreetNumber = reader.IsDBNull(1) ? null : reader.GetSqlDecimal(1).ToString(),
                            Street = reader.IsDBNull(2) ? null : reader.GetString(2),
                            City = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Zipcode = reader.IsDBNull(4) ? null : reader.GetString(4),
                        });

                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                purifiedAddress.AddressModel = list[i];
                nonPurifiedAddress.AddressModel = list[i];
                purifiedSelectResult = SelectCivilMapPurifiedAddress(purifiedAddress);

                //Already Exists In Purified, Don't Validate
                if (purifiedSelectResult.Count > 0)
                {
                    arrests.RemoveAt(i);
                    list.RemoveAt(i);
                    continue;
                }

                nonPurifiedSelectResult = SelectCivilMapNonPurifiedAddress(nonPurifiedAddress);
                if (nonPurifiedSelectResult.Count > 0)
                {
                    if (nonPurifiedSelectResult[0].PurifiedAddressId != null)
                    {
                        arrests.RemoveAt(i);
                        list.RemoveAt(i);
                    }
                    else
                    {
                        nonPurifiedAddressList.Add(new NonPurifiedAddressModel
                        {
                            NonPurifiedAddressId = nonPurifiedSelectResult[0].NonPurifiedAddressId,
                            AddressModel = list[i]
                        });
                    }
                }
                else
                {
                    Guid tempId = (Guid)AddCivilMapNonPurifiedAddress(nonPurifiedAddress);
                    nonPurifiedAddressList.Add(new NonPurifiedAddressModel
                    {
                        NonPurifiedAddressId = tempId,
                        AddressModel = list[i]
                    });
                }
            }

            //Validate

            for (int i = 0; i < nonPurifiedAddressList.Count; i++)
            {
                address = "";
                address += " " + nonPurifiedAddressList[i].AddressModel.StreetNumber + " " + nonPurifiedAddressList[i].AddressModel.Street + " " + nonPurifiedAddressList[i].AddressModel.City + " IL ";
                addresses.Add(address);
            }

            GeocodioClient.GeocodioClient geoClient = new GeocodioClient.GeocodioClient("3551a95da75a999c89a259153a77d2aa9a3d5a2");

            try
            {
                PurifiedAddressModel currentValidated = new PurifiedAddressModel();
                List<PurifiedAddressModel?> validatedAddresses = geoClient.BatchGeocode(addresses);

                for (int i = 0; i < validatedAddresses.Count; i++)
                {
                    if (validatedAddresses[i] != null)
                    {
                        currentValidated = (PurifiedAddressModel)validatedAddresses[i];
                        Guid newPurifiedId = (Guid)AddCivilMapPurifiedAddress(currentValidated);

                        NonPurifiedAddressModel updateModel = new NonPurifiedAddressModel();
                        updateModel.AddressModel = nonPurifiedAddressList[i].AddressModel;
                        updateModel.PurifiedAddressId = newPurifiedId;
                        updateModel.NonPurifiedAddressId = nonPurifiedAddressList[i].NonPurifiedAddressId;

                        UpdateCivilMapNonPurifiedAddress(updateModel);

                        InsertCivilMapArrestPurifiedAddress(arrests[i], newPurifiedId);
                    }

                }

                return validatedAddresses;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            return null;
        }

        public string AddCivilMapPoints(PointsModel item)
        {
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
                    Debug.WriteLine("Error: " + ex.Message);
                    Debug.WriteLine("Hits here!");
                }
                return null;
            }
        }

        public List<PointsModel> GetCivilMapPoints()
        {
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
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public List<PointsModel> SelectCivilMapPoints(PointsModel model)
        {
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
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public object InsertCivilMapArrestPurifiedAddress(string arrest, Guid address)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string commandText = "Insert into CivilMapArrestPurifiedAddress (Arrest_FK, PurifiedAddress_FK) output INSERTED.Id values " +
                                  "(@arrest, @address)";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@arrest", arrest);
                    command.Parameters.AddWithValue("@address", address);

                    object id = command.ExecuteScalar();
                    connection.Close();

                    InsertCivilMapPurifiedArrest(arrest, address);

                    return id;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return null;
            }
        }

        public object InsertCivilMapPurifiedArrest(string arrest, Guid address)
        {
            CivilMapCrimeArrestModel arrestModel = new CivilMapCrimeArrestModel();
            PurifiedAddressModel purifiedAddress = new PurifiedAddressModel();
            List<PurifiedAddressModel> purifiedAddressSelectReturn = new List<PurifiedAddressModel>();
            CivilMapArrestModel arrestSelectReturn = new CivilMapArrestModel();
            purifiedAddress.PurifiedAddressId = address;

            purifiedAddressSelectReturn = SelectCivilMapPurifiedAddressById(purifiedAddress);

            if(purifiedAddressSelectReturn.Count > 0)
            {
                arrestModel.PurifiedAddressId = address;
                arrestModel.Longitude = purifiedAddressSelectReturn[0].Longitude;
                arrestModel.Latitude = purifiedAddressSelectReturn[0].Latitude;
            } else
            {
                return null;
            }

            arrestSelectReturn = SelectArrestBeatDate(arrest);
            if(arrestSelectReturn.Arrest_Date != null && arrestSelectReturn.Cpd_Beat != null)
            {
                arrestModel.Date = arrestSelectReturn.Arrest_Date;
                arrestModel.Beat = arrestSelectReturn.Cpd_Beat;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string commandText = "Insert into CivilMapPurifiedArrest (Date, Longitude, Latitude, Beat, PurifiedAddressId) output INSERTED.Id values " +
                                  "(@Date, @Longitude, @Latitude, @Beat, @PurifiedAddressId)";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@Date", arrestModel.Date);
                    command.Parameters.AddWithValue("@Longitude", arrestModel.Longitude);
                    command.Parameters.AddWithValue("@Latitude", arrestModel.Latitude);
                    command.Parameters.AddWithValue("@Beat", arrestModel.Beat);
                    command.Parameters.AddWithValue("@PurifiedAddressId", arrestModel.PurifiedAddressId);

                    object id = command.ExecuteScalar();
                    connection.Close();

                    return id;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
                return null;
            }
        }

        public CivilMapArrestModel SelectArrestBeatDate(string arrest)
        {
            string commandText = "select ARREST_DATE, CPD_BEAT from CivilMapArrest where " +
                                 "ARREST_ID = @arrest";
            CivilMapArrestModel selectedArrest = new CivilMapArrestModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@arrest", arrest);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        selectedArrest.Arrest_Date = reader.GetDateTime(0);
                        selectedArrest.Cpd_Beat = reader.IsDBNull(1) ? null : reader.GetString(1);
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }               
            }
            return selectedArrest;

        }

        public void DeleteTable()
        {
            string commandText = "drop table [dbo].[KristinTest]";

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
                    Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public string RemoveStreetDescriptor (string address)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            int index = -1;

            if ((index = culture.CompareInfo.IndexOf(address, " AVE ", CompareOptions.IgnoreCase)) >= 0)
            {
                address = address.Remove(index, 4);
            }
            if ((index = culture.CompareInfo.IndexOf(address, " PL ", CompareOptions.IgnoreCase)) >= 0)
            {
                address = address.Remove(index, 3);
            }
            if ((index = culture.CompareInfo.IndexOf(address, " ST ", CompareOptions.IgnoreCase)) >= 0)
            {
                address = address.Remove(index, 3);
            }

            return address;
        }
    }
}