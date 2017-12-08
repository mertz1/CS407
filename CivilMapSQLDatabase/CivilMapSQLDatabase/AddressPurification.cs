using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.SqlTypes;

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
                                 "[Direction] NVARCHAR (384) NULL," +
                                 "[Street] NVARCHAR(384) NULL," +
                                 "[City] NVARCHAR(384) NULL," +
                                 "[Zipcode] NVARCHAR(384) NULL," +
                                 "[StreetType] NVARCHAR (384) NULL," +
                                 "[AddressType] NVARCHAR (384) NULL," +
                                 "[Longitude] DECIMAL(9, 6) NOT NULL," +
                                 "[Latitude] DECIMAL(9, 6) NOT NULL," +
                                 "[Polygon] NVARCHAR(5) NULL, " +
                                 "[State] NVARCHAR (384) NULL," +
                                 "PRIMARY KEY CLUSTERED([PurifiedAddressId] ASC))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                    commandText = "Insert into CivilMapPurifiedAddress (StreetNumber, Direction, Street, City, StreetType, Zipcode, AddressType, Longitude, Latitude, Polygon) output INSERTED.PurifiedAddressId values " +
                                  "(@StreetNumber, @Direction, @Street, @City, @StreetType, @Zipcode, @AddressType, @Longitude, @Latitude, @Polygon)";
                    command = new SqlCommand(commandText, connection);

                    command.Parameters.AddWithValue("@StreetNumber", model.AddressModel.StreetNumber);
                    command.Parameters.AddWithValue("@Direction", model.AddressModel.Direction?? "");
                    command.Parameters.AddWithValue("@Street", model.AddressModel.Street);
                    command.Parameters.AddWithValue("@City", model.AddressModel.City);
                    command.Parameters.AddWithValue("@Zipcode", model.AddressModel.Zipcode);
                    command.Parameters.AddWithValue("@StreetType", model.AddressModel.StreetType?? "");
                    command.Parameters.AddWithValue("@AddressType", model.AddressType?? "");
                    command.Parameters.AddWithValue("@Longitude", model.Longitude);
                    command.Parameters.AddWithValue("@Latitude", model.Latitude);
                    command.Parameters.AddWithValue("@Polygon", model.Polygon?? "");
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
                                StreetNumber = reader.IsDBNull(1)? null: reader.GetString(1),
                                Direction = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Street = reader.IsDBNull(3) ? null : reader.GetString(3),
                                StreetType = reader.IsDBNull(4) ? null : reader.GetString(4),
                                City = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Zipcode = reader.IsDBNull(6) ? null : reader.GetString(6)
                            },                           
                            AddressType = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Longitude = reader.GetDecimal(8),
                            Latitude = reader.GetDecimal(9),
                            Polygon = reader.IsDBNull(10) ? null : reader.GetString(10)
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
                                Direction = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Street = reader.IsDBNull(3) ? null : reader.GetString(3),
                                StreetType = reader.IsDBNull(6) ? null : reader.GetString(6),
                                City = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Zipcode = reader.IsDBNull(5) ? null : reader.GetString(5),
                                State = reader.IsDBNull(11) ? null : reader.GetString(11)
                            },                           
                            AddressType = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Longitude = reader.GetDecimal(8),
                            Latitude = reader.GetDecimal(9),
                            Polygon = reader.IsDBNull(10) ? null : reader.GetString(10)
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
                                 "StreetNumber = @StreetNumber and Direction = @Direction and Street = @Street and StreetType = @StreetType and City like @City and Zipcode = @Zipcode";
            var list = new List<PurifiedAddressModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@StreetNumber", model.AddressModel.StreetNumber);
                    command.Parameters.AddWithValue("@Direction", model.AddressModel.Direction ?? "");
                    command.Parameters.AddWithValue("@Street", (model.AddressModel.Street).ToString());
                    command.Parameters.AddWithValue("@StreetType", (model.AddressModel.StreetType)?.ToString() ?? "");
                    command.Parameters.AddWithValue("@City", (model.AddressModel.City).ToString());
                    command.Parameters.AddWithValue("@Zipcode", model.AddressModel.Zipcode ?? "");
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new PurifiedAddressModel
                        {
                            PurifiedAddressId = reader.GetGuid(0),
                            AddressModel = new AddressModel
                            {
                                StreetNumber = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Direction = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Street = reader.IsDBNull(3) ? null : reader.GetString(3),
                                StreetType = reader.IsDBNull(6) ? null : reader.GetString(6),
                                City = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Zipcode = reader.IsDBNull(5) ? null : reader.GetString(5),
                                State = reader.IsDBNull(11) ? null : reader.GetString(11)
                            },                         
                            AddressType = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Longitude = reader.GetDecimal(8),
                            Latitude = reader.GetDecimal(9),
                            Polygon = reader.IsDBNull(10) ? null : reader.GetString(10)
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


        public List<AddressPolygonModel> SelectArrestAddressPolygon()
        {
            string commandText = "select ARREST_ID, PurifiedAddress_FK, CPD_AREA, CPD_DISTRICT, CPD_SECTOR, CPD_BEAT from CivilMapArrest " +
                                 "inner join CivilMapArrestPurifiedAddress " +
                                 "on CivilMapArrest.ARREST_ID = CivilMapArrestPurifiedAddress.Arrest_FK";
            List<AddressPolygonModel> tempList = new List<AddressPolygonModel>();
            List<AddressPolygonModel> list = new List<AddressPolygonModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(commandText, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tempList.Add(new AddressPolygonModel
                        {
                            Id = reader.IsDBNull(0)? null : reader.GetString(0),
                            PurifiedAddressId = reader.GetGuid(1),
                            Area = reader.IsDBNull(2)? null : reader.GetString(2),
                            District = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Sector = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Beat = reader.IsDBNull(5) ? null : reader.GetString(5)
                        });
                    }

                    foreach (var item in tempList)
                    {
                        if(item.District != null)
                        {
                            string district = item.District.Substring(1, item.District.Length - 1);
                            string polygon = (item.Area + district + item.Sector + item.Beat).ToString();

                            list.Add(new AddressPolygonModel
                            {
                                PurifiedAddressId = item.PurifiedAddressId,
                                Polygon = polygon
                            });
                        }
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


        public List<AddressPolygonModel> SelectCrimeAddressPolygon()
        {
            string commandText = "select CivilMapCrime.ID, PurifiedAddress_FK, AREA, BEAT from CivilMapCrime " +
                                 "inner join CivilMapCrimePurifiedAddress " +
                                 "on CivilMapCrime.ID = CivilMapCrimePurifiedAddress.Crime_FK";
            List<AddressPolygonModel> tempList = new List<AddressPolygonModel>();
            List<AddressPolygonModel> list = new List<AddressPolygonModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(commandText, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tempList.Add(new AddressPolygonModel
                        {
                            Id = reader.IsDBNull(0) ? null : reader.GetString(0),
                            PurifiedAddressId = reader.GetGuid(1),
                            Area = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Beat = reader.IsDBNull(3) ? null : reader.GetString(3)
                        });
                    }

                    foreach (var item in tempList)
                    {
                        if (item.Beat != null)
                        {
                            string polygon = (item.Area + item.Beat).ToString();
                            list.Add(new AddressPolygonModel
                            {
                                PurifiedAddressId = item.PurifiedAddressId,
                                Polygon = polygon
                            });
                        }
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


        public void UpdateCivilMapPurifiedAddressOnPolygons(List<AddressPolygonModel> model)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    foreach (var item in model)
                    {
                        string commandText = "if exists (select * from CivilMapPurifiedAddress where PurifiedAddressId=@PurifiedAddressId) " +
                                         "update CivilMapPurifiedAddress set Polygon = @Polygon " +
                                         "where PurifiedAddressId=@PurifiedAddressId and Polygon is not null";
                        SqlCommand command = new SqlCommand(commandText, connection);
                        command.Parameters.AddWithValue("@PurifiedAddressId", item.PurifiedAddressId);
                        command.Parameters.AddWithValue("@Polygon", item.Polygon);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    Debug.WriteLine("Error in Update Purified Address On Polygons");
                }
            }
        }


        public object AddCivilMapNonPurifiedAddress(NonPurifiedAddressModel item)
        {
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapNonPurifiedAddress')" +
                                 "CREATE TABLE[dbo].[CivilMapNonPurifiedAddress](" +
                                 "[NonPurifiedAddressId] NUMERIC(18) NOT NULL," +
                                 "[StreetNumber] NVARCHAR(384) NULL," +
                                 "[Direction] NVARCHAR (384) NULL," +
                                 "[Street] NVARCHAR(384) NULL," +
                                 "[City] NVARCHAR(384) NULL," +
                                 "[Zipcode] NVARCHAR(384) NULL," +
                                 "[PurifiedAddressId] UNIQUEIDENTIFIER NOT NULL," +
                                 "[State] NVARCHAR(384) NULL," +
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

                    if (item.AddressModel.Direction == null)
                    {
                        item.AddressModel.Direction = "";
                    }
                    if (item.AddressModel.City == null)
                    {
                        item.AddressModel.City = "";
                    }
                    if (item.AddressModel.Zipcode == null)
                    {
                        item.AddressModel.Zipcode = "";
                    }
                    if (item.AddressModel.State == null)
                    {
                        item.AddressModel.State = "";
                    }


                    commandText = 
                                  "Insert into CivilMapNonPurifiedAddress (StreetNumber, Direction, Street, City, Zipcode, State) output INSERTED.NonPurifiedAddressId values " +
                                  "(@StreetNumber, @Direction, @Street, @City, @Zipcode, @State)";
                    command = new SqlCommand(commandText, connection);

                    command.Parameters.AddWithValue("@StreetNumber", item.AddressModel.StreetNumber);
                    command.Parameters.AddWithValue("@Direction", item.AddressModel.Direction);
                    command.Parameters.AddWithValue("@Street", item.AddressModel.Street);
                    command.Parameters.AddWithValue("@City", item.AddressModel.City);
                    command.Parameters.AddWithValue("@Zipcode", item.AddressModel.Zipcode);
                    command.Parameters.AddWithValue("@State", item.AddressModel.State);
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
                                Direction = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Street = reader.IsDBNull(3) ? null : reader.GetString(3),
                                City = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Zipcode = reader.IsDBNull(5) ? null : reader.GetString(5),
                            },
                            PurifiedAddressId = reader.IsDBNull(6) ? (Guid?)null: reader.GetGuid(6)
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
                                Direction = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Street = reader.IsDBNull(3) ? null : reader.GetString(3),
                                City = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Zipcode = reader.IsDBNull(5) ? null : reader.GetString(5),
                            },
                            PurifiedAddressId = reader.IsDBNull(6) ? (Guid?)null : reader.GetGuid(6)
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
            string commandText = "select * from CivilMapNonPurifiedAddress where StreetNumber = @StreetNumber and Street = @Street and City like @City";
            var list = new List<NonPurifiedAddressModel>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();

                    if (model.AddressModel.Zipcode == null)
                        model.AddressModel.Zipcode = "";

                    command.Parameters.AddWithValue("@StreetNumber", model.AddressModel.StreetNumber);
                    command.Parameters.AddWithValue("@Street", (model.AddressModel.Street).ToString());
                    command.Parameters.AddWithValue("@City", (model.AddressModel.City).ToString());

                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        list.Add(new NonPurifiedAddressModel
                        {
                            NonPurifiedAddressId = reader.GetGuid(0),
                            AddressModel = new AddressModel
                            {
                                StreetNumber = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Direction = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Street = reader.IsDBNull(3) ? null : reader.GetString(3),
                                City = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Zipcode = reader.IsDBNull(5) ? null : reader.GetString(5),
                                State = reader.IsDBNull(7) ? null : reader.GetString(7)
                            },
                            PurifiedAddressId = reader.IsDBNull(6) ? (Guid?)null : reader.GetGuid(6)
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
                                 "Insert into CivilMapNonPurifiedAddress (NonPurifiedAddressId, StreetNumber, Direction, Street, City, Zipcode, PurifiedAddressId) values " +
                                 "(@NonPurifiedAddressId, @StreetNumber, @Direction, @Street, @City, @Zipcode, @PurifiedAddressId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@NonPurifiedAddressId", model.NonPurifiedAddressId);
                    command.Parameters.AddWithValue("@StreetNumber", model.AddressModel.StreetNumber);
                    command.Parameters.AddWithValue("@Direction", model.AddressModel.Direction??"");
                    command.Parameters.AddWithValue("@Street", model.AddressModel.Street);
                    command.Parameters.AddWithValue("@City", model.AddressModel.City);
                    command.Parameters.AddWithValue("@Zipcode", model.AddressModel.Zipcode??"");
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
            string commandText = "select ARREST_ID, STREET_NO, STREET_DIRECTION_CD, STREET_NME, CITY, STATE_CD from CivilMapArrest ORDER BY ARREST_ID OFFSET 10100 ROWS FETCH NEXT 3000 ROWS ONLY";

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
            int existed = 0;

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
                            Direction = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Street = reader.IsDBNull(3) ? null : reader.GetString(3),
                            City = reader.IsDBNull(4) ? null : reader.GetString(4),
                            State = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Zipcode = null
                        });

                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Grabbed Data");

            for (int i = 0; i < list.Count; i++)
            {
                purifiedAddress.AddressModel = list[i];
                nonPurifiedAddress.AddressModel = list[i];
                purifiedSelectResult = SelectCivilMapPurifiedAddress(purifiedAddress);

                //Already Exists In Purified, Don't Validate
                if (purifiedSelectResult.Count > 0)
                {
                    existed++;
                    arrests.RemoveAt(i);
                    list.RemoveAt(i);
                    continue;
                }

                nonPurifiedSelectResult = SelectCivilMapNonPurifiedAddress(nonPurifiedAddress);
                if (nonPurifiedSelectResult.Count > 0)
                {
                    if (nonPurifiedSelectResult[0].PurifiedAddressId != null)
                    {
                        existed++;
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

            Console.WriteLine("Checked for existing");

            //Validate

            for (int i = 0; i < nonPurifiedAddressList.Count; i++)
            {
                address = "";
                address += " " + nonPurifiedAddressList[i].AddressModel.StreetNumber + " " + nonPurifiedAddressList[i].AddressModel.Direction + " " + nonPurifiedAddressList[i].AddressModel.Street + " " + nonPurifiedAddressList[i].AddressModel.City + " IL";// + //nonPurifiedAddressList[i].AddressModel.State;
                addresses.Add(address);
            }

            GeocodioClient.GeocodioClient geoClient = new GeocodioClient.GeocodioClient("3551a95da75a999c89a259153a77d2aa9a3d5a2");

            int count = 0;
            try
            {
                PurifiedAddressModel currentValidated = new PurifiedAddressModel();
                List<PurifiedAddressModel?> validatedAddresses = geoClient.BatchGeocode(addresses);

                Console.WriteLine("Validated Addresses");

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
                        count++;
                    }

                }

                Console.WriteLine("Addresses Added");


                Debug.WriteLine("Added: " + count + " addresses");
                Debug.WriteLine("Existed Already: " + existed);
                return validatedAddresses;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            return null;
        }

        public List<PurifiedAddressModel?> Validate100Crimes()
        {
            string commandText = "select ID, STNUM, STDIR, STREET, CITY, STATE_CD from CivilMapCrime ORDER BY ID OFFSET 400 ROWS FETCH NEXT 1000 ROWS ONLY";

            PurifiedAddressModel purifiedAddress = new PurifiedAddressModel();
            NonPurifiedAddressModel nonPurifiedAddress = new NonPurifiedAddressModel();
            List<String> crimes = new List<String>();
            List<AddressModel> list = new List<AddressModel>();
            List<PurifiedAddressModel> purifiedAddressList = new List<PurifiedAddressModel>();
            List<PurifiedAddressModel> purifiedSelectResult = new List<PurifiedAddressModel>();
            List<NonPurifiedAddressModel> nonPurifiedAddressList = new List<NonPurifiedAddressModel>();
            List<NonPurifiedAddressModel> nonPurifiedSelectResult = new List<NonPurifiedAddressModel>();
            List<String> addresses = new List<String>();
            string address;
            int fieldCount;
            int existed = 0;

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

                        crimes.Add(reader.GetString(0));
                        list.Add(new AddressModel
                        {
                            StreetNumber = reader.IsDBNull(1) ? null : reader.GetSqlDecimal(1).ToString(),
                            Direction = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Street = reader.IsDBNull(3) ? null : reader.GetString(3),
                            City = reader.IsDBNull(4) ? null : reader.GetString(4),
                            State = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Zipcode = null
                        });

                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Grabbed Data");

            for (int i = 0; i < list.Count; i++)
            {
                purifiedAddress.AddressModel = list[i];
                nonPurifiedAddress.AddressModel = list[i];
                purifiedSelectResult = SelectCivilMapPurifiedAddress(purifiedAddress);

                //Already Exists In Purified, Don't Validate
                if (purifiedSelectResult.Count > 0)
                {
                    existed++;
                    crimes.RemoveAt(i);
                    list.RemoveAt(i);
                    continue;
                }

                nonPurifiedSelectResult = SelectCivilMapNonPurifiedAddress(nonPurifiedAddress);
                if (nonPurifiedSelectResult.Count > 0)
                {
                    if (nonPurifiedSelectResult[0].PurifiedAddressId != null)
                    {
                        existed++;
                        crimes.RemoveAt(i);
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

            Console.WriteLine("Checked for existing");

            //Validate

            for (int i = 0; i < nonPurifiedAddressList.Count; i++)
            {
                address = "";
                address += " " + nonPurifiedAddressList[i].AddressModel.StreetNumber + " " + nonPurifiedAddressList[i].AddressModel.Direction + " " + nonPurifiedAddressList[i].AddressModel.Street + " " + nonPurifiedAddressList[i].AddressModel.City + " IL";// + //nonPurifiedAddressList[i].AddressModel.State;
                addresses.Add(address);
            }

            GeocodioClient.GeocodioClient geoClient = new GeocodioClient.GeocodioClient("3551a95da75a999c89a259153a77d2aa9a3d5a2");

            int count = 0;
            try
            {
                PurifiedAddressModel currentValidated = new PurifiedAddressModel();
                List<PurifiedAddressModel?> validatedAddresses = geoClient.BatchGeocode(addresses);

                Console.WriteLine("Validated Addresses");

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

                        InsertCivilMapCrimePurifiedAddress(crimes[i], newPurifiedId);
                        count++;
                    }

                }

                Console.WriteLine("Addresses Added");


                Debug.WriteLine("Added: " + count + " addresses");
                Debug.WriteLine("Existed Already: " + existed);
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

        public object InsertCivilMapCrimePurifiedAddress(string crime, Guid address)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string commandText = "Insert into CivilMapCrimePurifiedAddress (Crime_FK, PurifiedAddress_FK) output INSERTED.Id values " +
                                  "(@crime, @address)";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@crime", crime);
                    command.Parameters.AddWithValue("@address", address);

                    object id = command.ExecuteScalar();
                    connection.Close();

                    InsertCivilMapPurifiedCrime(crime, address);

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

                    if (arrestModel.Date == null)
                        arrestModel.Date = SqlDateTime.Null;
                    if(arrestModel.Beat == null)
                    {
                        arrestModel.Beat = "";
                    }

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

        public object InsertCivilMapPurifiedCrime(string crime, Guid address)
        {
            CivilMapCrimeArrestModel crimeModel = new CivilMapCrimeArrestModel();
            PurifiedAddressModel purifiedAddress = new PurifiedAddressModel();
            List<PurifiedAddressModel> purifiedAddressSelectReturn = new List<PurifiedAddressModel>();
            CivilMapArrestModel arrestSelectReturn = new CivilMapArrestModel();
            purifiedAddress.PurifiedAddressId = address;

            purifiedAddressSelectReturn = SelectCivilMapPurifiedAddressById(purifiedAddress);

            if (purifiedAddressSelectReturn.Count > 0)
            {
                crimeModel.PurifiedAddressId = address;
                crimeModel.Longitude = purifiedAddressSelectReturn[0].Longitude;
                crimeModel.Latitude = purifiedAddressSelectReturn[0].Latitude;
            }
            else
            {
                return null;
            }

            arrestSelectReturn = SelectArrestBeatDate(crime);
            if (arrestSelectReturn.Arrest_Date != null && arrestSelectReturn.Cpd_Beat != null)
            {
                crimeModel.Date = arrestSelectReturn.Arrest_Date;
                crimeModel.Beat = arrestSelectReturn.Cpd_Beat;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string commandText = "Insert into CivilMapPurifiedCrime (Date, Longitude, Latitude, Beat, PurifiedAddressId) output INSERTED.Id values " +
                                  "(@Date, @Longitude, @Latitude, @Beat, @PurifiedAddressId)";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();

                    if (crimeModel.Date == null)
                        crimeModel.Date = SqlDateTime.Null;
                    if (crimeModel.Beat == null)
                    {
                        crimeModel.Beat = "";
                    }

                    command.Parameters.AddWithValue("@Date", crimeModel.Date);
                    command.Parameters.AddWithValue("@Longitude", crimeModel.Longitude);
                    command.Parameters.AddWithValue("@Latitude", crimeModel.Latitude);
                    command.Parameters.AddWithValue("@Beat", crimeModel.Beat);
                    command.Parameters.AddWithValue("@PurifiedAddressId", crimeModel.PurifiedAddressId);

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