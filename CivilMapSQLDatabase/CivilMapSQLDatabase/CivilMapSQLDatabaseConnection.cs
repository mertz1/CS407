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
    public partial class CivilMapSQLDatabaseConnection : IDisposable
    {
        static void Main(string[] args)
        {
            CrimeAndArrestDataRetrieve crimeAndArrestDataRetrieve = new CrimeAndArrestDataRetrieve();
            AddressPurification addressPurification = new AddressPurification();
            VisualizationDataAggregation visualizationDataAggregation = new VisualizationDataAggregation();

            //var address = new PurifiedAddressModel();
            //address.AddressModel.Street = "Waldron Street";
            //address.AddressModel.StreetNumber = "417";
            //address.AddressModel.Zipcode = "47906";
            //addressPurification.ValidateAddress(address);


            var lon = -87.743611;
            var lat = 41.116365;
            var rad = 2;
            visualizationDataAggregation.SelectCivilMapPurifiedAddress(lon, lat, rad);


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
                    AddressModel = new AddressModel
                    {
                        StreetNumber = (i * 10 + i / 2.0).ToString(),
                        Street = street.ElementAt(i % 5),
                        City = "Chicago",
                        Zipcode = zipcode.ElementAt(i % 7)
                    },
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
                    AddressModel = new AddressModel
                    {
                        StreetNumber = (i * 10 + i / 2.0).ToString(),
                        Street = street.ElementAt(i % 5),
                        City = "Chicago",
                        Zipcode = zipcode.ElementAt(i % 7)
                    }
                };
                if (i % 5 == 0)
                    model.PurifiedAddressId = Guid.Parse("3bec84e5-9683-4d27-94e3-102998572a7d");
                list.Add(model);
            }
            return list;
        }
    }
}
