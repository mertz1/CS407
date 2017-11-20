using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilMapSQLDatabase
{
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

        public struct AddressModel
        {
            public string StreetNumber;
            public string Direction;
            public string Street;
            public string City;
            public string Zipcode;
        };

        public struct PurifiedAddressModel
        {
            public Guid PurifiedAddressId;   // PK
            public AddressModel AddressModel;
            public string StreetType;
            public string AddressType;
            public Decimal Longitude;
            public Decimal Latitude;
        };

        public struct NonPurifiedAddressModel
        {
            public Guid NonPurifiedAddressId;    // PK
            public AddressModel AddressModel;
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
        };

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
