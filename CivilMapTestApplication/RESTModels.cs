using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilMapTestApplication
{
    public class AddressRESTModel
    {
        public string StreetNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
    }

    public class PurifiedAddressRESTModel
    {
        public string PurifiedAddressId { get; set; }
        public AddressRESTModel AddressRESTModel { get; set; }
        public Decimal Longitude { get; set; }
        public Decimal Latitude { get; set; }
    }

    public class NonPurifiedAddressRESTModel
    {
        public string NonPurifiedAddressId { get; set; }
        public AddressRESTModel AddressRESTModel { get; set; }
        public string PurifiedAddressId { get; set; }
    }

    public class PointsRESTModel
    {
        public string PointId { get; set; }
        public string PurifiedAddressId { get; set; }
    }
}
