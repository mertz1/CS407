using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace CivilMapRESTApi.Models
{
    public class PurifiedAddressRESTModel
    {
        public string PurifiedAddressId { get; set; }
        public AddressRESTModel AddressRESTModel { get; set; }
        public string StreetType { get; set; }
        public string AddressType { get; set; }
        public Decimal Longitude { get; set; }
        public Decimal Latitude { get; set; }
        public string Polygon { get; set; }
    }
}