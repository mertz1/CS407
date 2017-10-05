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


            var address = new PurifiedAddressModel();
            address.Street = "Waldron Street";
            address.StreetNumber = "417";
            address.Zipcode = "47906";

            addressPurification.ValidateAddress(address);


            //var models = PurifiedModelPrepare();
            //foreach(var item in models)
            //{
            //    var list = addressPurification.SelectCivilMapPurifiedAddress(item);
            //    if(list.Count == 0 || list == null)
            //    {
            //        string str = addressPurification.AddCivilMapPurifiedAddress(item);
            //        Console.WriteLine(str);
            //    }
            //    else
            //    {
            //        Console.WriteLine("Existed: ");
            //        foreach(var ll in list)
            //        {
            //            Console.WriteLine("     " + ll.PurifiedAddressId);
            //        }
            //    }
            //}



            //var models = NonPurifiedAddressModelPrepare();
            //foreach (var item in models)
            //{
            //    var list = addressPurification.SelectCivilMapNonPurifiedAddress(item);
            //    if (list.Count == 0 || list == null)
            //    {
            //        string str = addressPurification.AddCivilMapNonPurifiedAddress(item);
            //        Console.WriteLine(str);
            //    }
            //    else
            //    {
            //        Console.WriteLine("Existed: ");
            //        foreach (var ll in list)
            //        {
            //            Console.WriteLine("     " + ll.NonPurifiedAddressId);
            //        }
            //    }
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
                    PurifiedAddressId = Guid.Parse("f8592e81-2dff-48c0-b005-0068df163e0e")
                });
            }
            return list;
        }

        private static List<PurifiedAddressModel> PurifiedModelPrepare()
        {
            List<PurifiedAddressModel> list = new List<PurifiedAddressModel>();
            List<string> street = new List<string>
            {
                "Martin Jishcke",
                "Traft Rd",
                "Tippenacnoe Mall",
                "Russel St",
                "HICKS underground"
            };
            List<string> zipcode = new List<string> { "60606", "60302", "60537", "68970", "60412", "66205", "63284" };

            for (int i = 0; i < 20; i++)
            {
                list.Add(new PurifiedAddressModel
                {
                    StreetNumber = (i * 10 + i / 2.0).ToString(),
                    Street = street.ElementAt(i % 5),
                    City = "Chicago",
                    Zipcode = zipcode.ElementAt(i % 7),
                    Longitude = Convert.ToDecimal(41.06 + i/156.89),
                    Latitude = Convert.ToDecimal(-87.81568 + i / 145.0)
                });
            }
            return list;
        }

        private static List<NonPurifiedAddressModel> NonPurifiedAddressModelPrepare()
        {
            List<NonPurifiedAddressModel> list = new List<NonPurifiedAddressModel>();
            List<string> street = new List<string>
            {
                "Lawson Computer Science",
                "Purdue Memorial Union",
                "Stewart Center",
                "Beering Hall",
                "Earhart Dining Court"
            };
            List<string> zipcode = new List<string> { "60606", "60302", "60537", "68970", "60412", "66205", "63284" };

            for (int i = 0; i < 20; i++)
            {
                NonPurifiedAddressModel model = new NonPurifiedAddressModel
                {
                    StreetNumber = (i * 10 + i / 2.0).ToString(),
                    Street = street.ElementAt(i % 5),
                    City = "Chicago",
                    Zipcode = zipcode.ElementAt(i % 7)
                };
                if (i % 5 == 0)
                    model.PurifiedAddressId = Guid.Parse("3bec84e5-9683-4d27-94e3-102998572a7d");
                list.Add(model);
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
            public Guid PointId;    // PK
            public Guid? PurifiedAddressId;   // FK
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
            public Guid NonPurifiedAddressId;    // PK
            public string StreetNumber;
            public string Street;
            public string City;
            public string Zipcode;
            public Guid? PurifiedAddressId;   // FK
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
