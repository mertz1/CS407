using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CivilMapSQLDatabase
{
    public partial class CivilMapSQLDatabaseConnection : IDisposable
    {
        static void Main(string[] args)
        {
            CrimeAndArrestDataRetrieve crimeAndArrestDataRetrieve = new CrimeAndArrestDataRetrieve();
            AddressPurification addressPurification = new AddressPurification();

            //var list = crimeAndArrestDataRetrieve.GetCivilMapArrest();
            //var model = new List<CivilMapCrimeArrestModel>();
            //foreach (var item in list)
            //{
            //    model.Add(new CivilMapCrimeArrestModel
            //    {
            //        Id = item.Arrest_Id,
            //        Date = item.Arrest_Date,
            //        Longitude = null,
            //        Latitude = null,
            //        Beat = item.Cpd_Beat,
            //        PurifiedAddressId = null
            //    });
            //}
            //crimeAndArrestDataRetrieve.AddCivilMapPurifiedArrest(model);
            //System.Diagnostics.Debug.WriteLine("Finished Insertion");

            //List<double> longitude = new List<double>();
            //longitude.Add(-87.622083);
            //longitude.Add(-87.610530);
            //longitude.Add(-87.623189);
            //longitude.Add(-87.614035);
            //longitude.Add(-87.616823);

            //List<double> latitude = new List<double>();
            //latitude.Add(41.754730);
            //latitude.Add(41.766657);
            //latitude.Add(41.755658);
            //latitude.Add(41.766254);
            //latitude.Add(41.773602);

            //for (int i = 0; i < 20; i++)
            //{
            //    DateTime date;
            //    string lo, la;
            //    string id = ("002" + i).ToString();
            //    string beat = ("36" + 2 * i).ToString();
            //    if(i < 5)
            //    {
            //        date = new DateTime(2014, 01, 01);
            //        lo = (longitude.ElementAt(i % 5) + i / 550.5).ToString();
            //        la = (latitude.ElementAt(i % 5) + i / 230.5).ToString();
            //    }
            //    else if (i < 10)
            //    {
            //        date = new DateTime(2014, 01, 02);
            //        lo = (longitude.ElementAt(i % 5) - i / 550.5).ToString();
            //        la = (latitude.ElementAt(i % 5) - i / 230.5).ToString();
            //    }

            //    else if (i < 15)
            //    {
            //        date = new DateTime(2014, 01, 03);
            //        lo = (longitude.ElementAt(i % 5) + i / 550.5).ToString();
            //        la = (latitude.ElementAt(i % 5) + i / 230.5).ToString();
            //    }  
            //    else
            //    {
            //        date = new DateTime(2014, 01, 04);
            //        lo = (longitude.ElementAt(i % 5) - i / 550.5).ToString();
            //        la = (latitude.ElementAt(i % 5) - i / 230.5).ToString();
            //    } 
            //    //crimeAndArrestDataRetrieve.AddTestCrime(id, date, lo, la, beat);
            //    crimeAndArrestDataRetrieve.AddTestArrest(id, date, lo, la, beat);
            //}





            //crimeAndArrestDataRetrieve.GetCivilMapCrime();
            //crimeAndArrestDataRetrieve.GetCivilMapArrest();

            //addressPurification.DeleteTable();
            //addressPurification.AddKristinTest(PurifiedAddressModelPrepare());

            //addressPurification.AddCivilMapPurifiedAddress(PurifiedAddressModelPrepare());
            //addressPurification.AddCivilMapPoints(PointsModelPrepare());
            //addressPurification.AddCivilMapNonPurifiedAddress(NonPurifiedAddressModelPrepare());

            //addressPurification.GetCivilMapPurifiedAddress();
            //addressPurification.GetCivilMapNonPurifiedAddress();
            //addressPurification.GetCivilMapPoints();

            //addressPurification.SelectCivilMapPurifiedAddress("cf4ac28b-d847-45f5-849c-15a7e6820227", "hah", Convert.ToDecimal(125.36), Convert.ToDecimal(468.25));
            //addressPurification.SelectCivilMapNonPurifiedAddress(125568, "din", "cf4ac28b-d847-45f5-849c-15a7e6820227");
            //addressPurification.SelectCivilMapPoints(56896, "f8592e81-2dff-48c0-b005-0068df163e0e");

            //addressPurification.UpdateCivilMapNonPurifiedAddress(100, "Lawson computer science building", "ca173871-2ef4-4eff-a3c2-241cb1500d4b");
        }

        void IDisposable.Dispose()
        {
           
        }

        private static List<PointsModel> PointsModelPrepare()
        {
            List<PointsModel> list = new List<PointsModel>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(new PointsModel
                {
                    PointId = 56892 + i,
                    PurifiedAddressId = Guid.Parse("f8592e81-2dff-48c0-b005-0068df163e0e")
                });
            }
            return list;
        }
    }

    partial class CivilMapSQLDatabaseConnection
    {
        public struct TestTableModel
        {
            public Int32 id;
            public string name;
        };

        public struct PointsModel
        {
            public Decimal PointId;    // PK
            public Guid PurifiedAddressId;   // FK
        };
        
        public struct PurifiedAddressModel
        {
            public Guid PurifiedAddressId;   // PK
            public string StreetNumber;
            public string Street;
            public string City;
            public string Zipcode;
            public Decimal Longitude;
            public Decimal Latitude;
        };

        public struct NonPurifiedAddressModel
        {
            public Decimal NonPurifiedAddressId;    // PK
            public string StreetNumber;
            public string Street;
            public string City;
            public string Zipcode;
            public Guid PurifiedAddressId;   // FK
        };

        public struct CivilMapCrimeArrestModel
        {
            public string Id;   // PK
            public DateTime? Date;
            public decimal? Longitude;
            public decimal? Latitude;
            public string Beat;
            public Guid? PurifiedAddressId;
        }

        public struct CivilMapCrimeModel
        {
            public string Status;
            public string BeatAsgn;
            public DateTime? DateOcc;
            public string Fire_I;
            public string Gung_I;
            public string Domestic_I;
            public string Vehicle_Theft_I;
            public decimal? No_Of_Offenders;
            public string Area;
            public string District;
            public string Beat;
            public decimal? StNum;
            public string StDir;
            public string Street;
            public string Apt_No;
            public string City;
            public string State_Cd;
            public decimal? Geo_Code_Id;
            public string Curr_Iucr;
            public string Primary;
            public string Description;
            public string Id;    // PK
            public string Vic_Cnt;
            public string Fbi_Cd;
            public string Fbi_Descr;
            public string Fbi_Indx;
            public string Fbi_Vpn;
            public string Location_Descr;
            public string Location_Sec_Descr;
            public string Method_Descr;
            public string Cau_Descr;
            public string Motive_Descr;
            public string Cause_Descr;
            public string Entry_Descr;
            public string Case_Report_Id;
            public string Shootings_I;
        };

        public struct CivilMapArrestModel
        {
            public string Arrest_Id; // PK   
            public string Offender_Id;
            public DateTime? Arrest_Date;
            public string Arr_District;
            public string Cpd_District;
            public string Cpd_Sector;
            public string Cpd_Beat;
            public string Cpd_Area;
            public string Arresting_Unit;
            public string Arresting_Beat;
            public decimal? Street_No;
            public string Street_Direction_Cd;
            public string Street_Nme;
            public string Apt_No;
            public string City;
            public string State_Cd;
            public string Zip_Cd;
            public string County_Cd;
            public string Arr_Charge_Id;
            public decimal? Charge_Code_Id;
            public string Statute;
            public string Stat_Descr;
            public string Charge_Class_Cd;
            public string Charge_Type_Cd;
            public string Iucr_Code_Cd;
            public string Primary_Class;
            public string Secondary_Class;
            public string Cb_No;
            public string Ir_No;
            public decimal? Sid_No;
            public string Fbi_No;
            public string Fbi_Code;
            public string Inchoate_Code_Cd;
        };
    }
}
