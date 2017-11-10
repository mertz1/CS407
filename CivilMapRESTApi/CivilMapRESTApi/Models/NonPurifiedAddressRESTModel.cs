using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CivilMapRESTApi.Models
{
    public class NonPurifiedAddressRESTModel
    {
        public string NonPurifiedAddressId { get; set; }
        public AddressRESTModel AddressRESTModel { get; set; }
        public string PurifiedAddressId { get; set; }
    }
}