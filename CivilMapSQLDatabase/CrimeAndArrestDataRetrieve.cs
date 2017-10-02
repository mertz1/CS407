using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CivilMapSQLDatabase
{
    class CrimeAndArrestDataRetrieve : CivilMapSQLDatabaseConnection
    {
        public CrimeAndArrestDataRetrieve()
        {
            
        }

        //////////////////////////////////////////// <summary>
        public void AddTestCrime(CivilMapCrimeArrestModel model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "Insert into [dbo].[TestCrime] (Id, Date, Longitude, Latitude, Beat) values " +
                        "(@Id, @Date, @Longitude, @Latitude, @Beat)"; ;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@Id", model.Id);
                    command.Parameters.AddWithValue("@Date", model.Date);
                    command.Parameters.AddWithValue("@Longitude", model.Longitude);
                    command.Parameters.AddWithValue("@Latitude", model.Latitude);
                    command.Parameters.AddWithValue("@Beat", model.Beat);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public void AddTestArrest(CivilMapCrimeArrestModel model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "Insert into [dbo].[TestArrest] (Id, Date, Longitude, Latitude, Beat) values " +
                        "(@Id, @Date, @Longitude, @Latitude, @Beat)"; ;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@Id", model.Id);
                    command.Parameters.AddWithValue("@Date", model.Date);
                    command.Parameters.AddWithValue("@Longitude", model.Longitude);
                    command.Parameters.AddWithValue("@Latitude", model.Latitude);
                    command.Parameters.AddWithValue("@Beat", model.Beat);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }
        ///////////////////////////////////////////// </summary>
        ///////////////////////////////////////////// <returns></returns>

        public List<TestTableModel> GetDataFromTestTable()
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "select * from testtable";
            var list = new List<TestTableModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new TestTableModel
                        {
                            id = reader.GetInt32(0),
                            name = reader.GetString(1)
                        });
                    }
                    System.Diagnostics.Debug.WriteLine("Finished reading testtable");
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public void AddCivilMapPurifiedCrime(CivilMapCrimeArrestModel model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapPurifiedCrime')" +
                                 "CREATE TABLE[dbo].[CivilMapPurifiedCrime] ( " + 
                                 "[Id] NVARCHAR(384) NOT NULL PRIMARY KEY, " + 
                                 "[Date] DATETIME2 NULL, " + 
                                 "[Longitude] DECIMAL(9, 6) NULL, " + 
                                 "[Latitude] DECIMAL(9, 6) NULL, " +
                                 "[Beat] NVARCHAR(384) NULL, " +
                                 "[PurifiedAddressId] UNIQUEIDENTIFIER NULL, " + 
                                 "CONSTRAINT[FK_CivilMapPurifiedCrime_CivilMapPurifiedAddress] FOREIGN KEY([PurifiedAddressId]) REFERENCES[CivilMapPurifiedAddress] ([PurifiedAddressId]))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                    commandText = "if not exists (select * from CivilMapPurifiedCrime where Id=@Id)" +
                        "Insert into CivilMapPurifiedCrime (Id, Date, Longitude, Latitude, Beat, PurifiedAddressId) values " +
                        "(@Id, @Date, @Longitude, @Latitude, @Beat, @PurifiedAddressId)";
                    command = new SqlCommand(commandText, connection);

                    command.Parameters.AddWithValue("@Id", model.Id);
                    command.Parameters.AddWithValue("@Date", model.Date?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Longitude", model.Longitude?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Latitude", model.Latitude?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Beat", model.Beat ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PurifiedAddressId", model.PurifiedAddressId?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public void AddCivilMapPurifiedCrime(List<CivilMapCrimeArrestModel> model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapPurifiedCrime')" +
                                 "CREATE TABLE[dbo].[CivilMapPurifiedCrime] ( " +
                                 "[Id] NVARCHAR(384) NOT NULL PRIMARY KEY, " +
                                 "[Date] DATETIME2 NULL, " +
                                 "[Longitude] DECIMAL(9, 6) NULL, " +
                                 "[Latitude] DECIMAL(9, 6) NULL, " +
                                 "[Beat] NVARCHAR(384) NULL, " +
                                 "[PurifiedAddressId] UNIQUEIDENTIFIER NULL, " +
                                 "CONSTRAINT[FK_CivilMapPurifiedCrime_CivilMapPurifiedAddress] FOREIGN KEY([PurifiedAddressId]) REFERENCES[CivilMapPurifiedAddress] ([PurifiedAddressId]))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                    foreach(var item in model)
                    {
                        commandText = "if not exists (select * from CivilMapPurifiedCrime where Id=@Id)" +
                        "Insert into CivilMapPurifiedCrime (Id, Date, Longitude, Latitude, Beat, PurifiedAddressId) values " +
                        "(@Id, @Date, @Longitude, @Latitude, @Beat, @PurifiedAddressId)";
                        command = new SqlCommand(commandText, connection);

                        command.Parameters.AddWithValue("@Id", item.Id);
                        command.Parameters.AddWithValue("@Date", item.Date ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Longitude", item.Longitude ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Latitude", item.Latitude ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Beat", item.Beat ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PurifiedAddressId", item.PurifiedAddressId ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public void AddCivilMapPurifiedArrest(CivilMapCrimeArrestModel model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapPurifiedArrest')" +
                                 "CREATE TABLE[dbo].[CivilMapPurifiedArrest] ( " +
                                 "[Id] NVARCHAR(384) NOT NULL PRIMARY KEY, " +
                                 "[Date] DATETIME2 NULL, " +
                                 "[Longitude] DECIMAL(9, 6) NULL, " +
                                 "[Latitude] DECIMAL(9, 6) NULL, " +
                                 "[Beat] NVARCHAR(384) NULL, " +
                                 "[PurifiedAddressId] UNIQUEIDENTIFIER NULL, " +
                                 "CONSTRAINT[FK_CivilMapPurifiedArrest_CivilMapPurifiedAddress] FOREIGN KEY([PurifiedAddressId]) REFERENCES[CivilMapPurifiedAddress] ([PurifiedAddressId]))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                    commandText = "if not exists (select * from CivilMapPurifiedArrest where Id=@Id)" +
                        "Insert into CivilMapPurifiedArrest (Id, Date, Longitude, Latitude, Beat, PurifiedAddressId) values " +
                        "(@Id, @Date, @Longitude, @Latitude, @Beat, @PurifiedAddressId)";
                    command = new SqlCommand(commandText, connection);

                    command.Parameters.AddWithValue("@Id", model.Id);
                    command.Parameters.AddWithValue("@Date", model.Date ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Longitude", model.Longitude ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Latitude", model.Latitude ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Beat", model.Beat ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PurifiedAddressId", model.PurifiedAddressId ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public void AddCivilMapPurifiedArrest(List<CivilMapCrimeArrestModel> model)
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "if not exists (select * from INFORMATION_SCHEMA.TABLES where Table_Name='CivilMapPurifiedArrest')" +
                                 "CREATE TABLE[dbo].[CivilMapPurifiedArrest] ( " +
                                 "[Id] NVARCHAR(384) NOT NULL PRIMARY KEY, " +
                                 "[Date] DATETIME2 NULL, " +
                                 "[Longitude] DECIMAL(9, 6) NULL, " +
                                 "[Latitude] DECIMAL(9, 6) NULL, " +
                                 "[Beat] NVARCHAR(384) NULL, " +
                                 "[PurifiedAddressId] UNIQUEIDENTIFIER NULL, " +
                                 "CONSTRAINT[FK_CivilMapPurifiedArrest_CivilMapPurifiedAddress] FOREIGN KEY([PurifiedAddressId]) REFERENCES[CivilMapPurifiedAddress] ([PurifiedAddressId]))";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    command.ExecuteNonQuery();

                    foreach(var item in model)
                    {
                        commandText = "if not exists (select * from CivilMapPurifiedArrest where Id=@Id)" +
                        "Insert into CivilMapPurifiedArrest (Id, Date, Longitude, Latitude, Beat, PurifiedAddressId) values " +
                        "(@Id, @Date, @Longitude, @Latitude, @Beat, @PurifiedAddressId)";
                        command = new SqlCommand(commandText, connection);

                        command.Parameters.AddWithValue("@Id", item.Id);
                        command.Parameters.AddWithValue("@Date", item.Date ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Longitude", item.Longitude ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Latitude", item.Latitude ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Beat", item.Beat ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PurifiedAddressId", item.PurifiedAddressId ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public List<CivilMapCrimeModel> GetCivilMapCrime()
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "select * from CivilMapCrime";
            //string commandText = "select top 10 * from CivilMapCrime";
            var list = new List<CivilMapCrimeModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new CivilMapCrimeModel
                        {
                            Status = reader.IsDBNull(0) ? null : reader.GetString(0),
                            BeatAsgn = reader.IsDBNull(1) ? null : reader.GetString(1),
                            DateOcc = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            Fire_I = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Gung_I = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Domestic_I = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Vehicle_Theft_I = reader.IsDBNull(6) ? null : reader.GetString(6),
                            No_Of_Offenders = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                            Area = reader.IsDBNull(8) ? null : reader.GetString(8),
                            District = reader.IsDBNull(9) ? null : reader.GetString(9),
                            Beat = reader.IsDBNull(10) ? null : reader.GetString(10),
                            StNum = reader.IsDBNull(11) ? (decimal?)null : reader.GetDecimal(11),
                            StDir = reader.IsDBNull(12) ? null : reader.GetString(12),
                            Street = reader.IsDBNull(13) ? null : reader.GetString(13),
                            Apt_No = reader.IsDBNull(14) ? null : reader.GetString(14),
                            City = reader.IsDBNull(15) ? null : reader.GetString(15),
                            State_Cd = reader.IsDBNull(16) ? null : reader.GetString(16),
                            Geo_Code_Id = reader.IsDBNull(17) ? (decimal?)null : reader.GetDecimal(17),
                            Curr_Iucr = reader.IsDBNull(18) ? null : reader.GetString(18),
                            Primary = reader.IsDBNull(19) ? null : reader.GetString(19),
                            Description = reader.IsDBNull(20) ? null : reader.GetString(20),
                            Id = reader.IsDBNull(21) ? null : reader.GetString(21),
                            Vic_Cnt = reader.IsDBNull(22) ? null : reader.GetString(22),
                            Fbi_Cd = reader.IsDBNull(23) ? null : reader.GetString(23),
                            Fbi_Descr = reader.IsDBNull(24) ? null : reader.GetString(24),
                            Fbi_Indx = reader.IsDBNull(25) ? null : reader.GetString(25),
                            Fbi_Vpn = reader.IsDBNull(26) ? null : reader.GetString(26),
                            Location_Descr = reader.IsDBNull(27) ? null : reader.GetString(27),
                            Location_Sec_Descr = reader.IsDBNull(28) ? null : reader.GetString(28),
                            Method_Descr = reader.IsDBNull(29) ? null : reader.GetString(29),
                            Cau_Descr = reader.IsDBNull(30) ? null : reader.GetString(30),
                            Motive_Descr = reader.IsDBNull(31) ? null : reader.GetString(31),
                            Cause_Descr = reader.IsDBNull(32) ? null : reader.GetString(32),
                            Entry_Descr = reader.IsDBNull(33) ? null : reader.GetString(33),
                            Case_Report_Id = reader.IsDBNull(34) ? null : reader.GetString(34),
                            Shootings_I = reader.IsDBNull(35) ? null : reader.GetString(35)
                        });
                    }

                    System.Diagnostics.Debug.WriteLine("Finished reading CivilMapCrime");
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }

        public List<CivilMapArrestModel> GetCivilMapArrest()
        {
            string connectionString = "Data Source=tcp:civilmapdb.database.windows.net,1433;Initial Catalog=civilmapdb-dev;Persist Security Info=False;User ID=civilmapuser;Password=M#apitright95;Connect Timeout=30;Encrypt=True";
            string commandText = "select * from CivilMapArrest";
            //string commandText = "select top 200 * from CivilMapArrest";
            var list = new List<CivilMapArrestModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(new CivilMapArrestModel
                        {
                            Arrest_Id = reader.IsDBNull(0) ? null : reader.GetString(0),
                            Offender_Id = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Arrest_Date = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            Arr_District = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Cpd_District = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Cpd_Sector = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Cpd_Beat = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Cpd_Area = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Arresting_Unit = reader.IsDBNull(8) ? null : reader.GetString(8),
                            Arresting_Beat = reader.IsDBNull(9) ? null : reader.GetString(9),
                            Street_No = reader.IsDBNull(10) ? (decimal?)null : reader.GetDecimal(10),
                            Street_Direction_Cd = reader.IsDBNull(11) ? null : reader.GetString(11),
                            Street_Nme = reader.IsDBNull(12) ? null : reader.GetString(12),
                            Apt_No = reader.IsDBNull(13) ? null : reader.GetString(13),
                            City = reader.IsDBNull(14) ? null : reader.GetString(14),
                            State_Cd = reader.IsDBNull(15) ? null : reader.GetString(15),
                            Zip_Cd = reader.IsDBNull(16) ? null : reader.GetString(16),
                            County_Cd = reader.IsDBNull(17) ? null : reader.GetString(17),
                            Arr_Charge_Id = reader.IsDBNull(18) ? null : reader.GetString(18),
                            Charge_Code_Id = reader.IsDBNull(19) ? (decimal?)null : reader.GetDecimal(19),
                            Statute = reader.IsDBNull(20) ? null : reader.GetString(20),
                            Stat_Descr = reader.IsDBNull(21) ? null : reader.GetString(21),
                            Charge_Class_Cd = reader.IsDBNull(22) ? null : reader.GetString(22),
                            Charge_Type_Cd = reader.IsDBNull(23) ? null : reader.GetString(23),
                            Iucr_Code_Cd = reader.IsDBNull(24) ? null : reader.GetString(24),
                            Primary_Class = reader.IsDBNull(25) ? null : reader.GetString(25),
                            Secondary_Class = reader.IsDBNull(26) ? null : reader.GetString(26),
                            Cb_No = reader.IsDBNull(27) ? null : reader.GetString(27),
                            Ir_No = reader.IsDBNull(28) ? null : reader.GetString(28),
                            Sid_No = reader.IsDBNull(29) ? (decimal?)null : reader.GetDecimal(29),
                            Fbi_No = reader.IsDBNull(30) ? null : reader.GetString(30),
                            Fbi_Code = reader.IsDBNull(31) ? null : reader.GetString(31),
                            Inchoate_Code_Cd = reader.IsDBNull(32) ? null : reader.GetString(32)
                        });
                    }

                    System.Diagnostics.Debug.WriteLine("Finished reading CivilMapArrest");
                    connection.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return list;
            }
        }
    }
}
