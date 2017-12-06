using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilMapTestApplication
{
    partial class TestApplication
    {
        static PurifiedAddressRESTModel CreatePurifiedAddressRESTModel()
        {
            PurifiedAddressRESTModel model = new PurifiedAddressRESTModel
            {
                AddressRESTModel = new AddressRESTModel
                {
                    StreetNumber = "49",
                    Direction = "",
                    Street = "Wall St",
                    City = "Chicago",
                    Zipcode = "60606"
                },
                StreetType = "AVE",
                AddressType = "Interaction",
                Longitude = Convert.ToDecimal(-87.761326),
                Latitude = Convert.ToDecimal(41.117596),
                Polygon = "31125"
            };
            return model;
        }

        static NonPurifiedAddressRESTModel CreateNonPurifiedAddressRESTModel()
        {
            NonPurifiedAddressRESTModel model = new NonPurifiedAddressRESTModel
            {
                AddressRESTModel = new AddressRESTModel
                {
                    StreetNumber = "250",
                    Direction = "",
                    Street = "Baker St",
                    City = "Chicago",
                    Zipcode = "60215"
                },
                PurifiedAddressId = ""
            };
            return model;
        }

        static NonPurifiedAddressRESTModel CreateNonPurifiedAddressForUpdate()
        {
            NonPurifiedAddressRESTModel model = new NonPurifiedAddressRESTModel
            {
                NonPurifiedAddressId = "18094643-16dc-4a48-b661-4e72f5ed8149",
                AddressRESTModel = new AddressRESTModel
                {
                    StreetNumber = "120", 
                    Direction = "",
                    Street = "North Michigan Avenue",
                    City = "Chicago",
                    Zipcode = "60123"
                },
                PurifiedAddressId = "af7ad6fb-4e18-477b-a708-61eae06e0cd2"
            };
            return model;
        }

        static PointsRESTModel CreatePointsRESTModel()
        {
            PointsRESTModel model = new PointsRESTModel
            {
                PurifiedAddressId = "0883dd1e-9ef9-4c1b-b78c-fc2e4487e9ab"
            };
            return model;
        }
    }
}
