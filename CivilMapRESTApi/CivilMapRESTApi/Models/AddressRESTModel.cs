using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CivilMapRESTApi.Models
{
    public class AddressRESTModel
    {
        public string StreetNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
    }
}