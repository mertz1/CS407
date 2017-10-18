using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilMapTestApplication
{
    class CheckAuthorization
    {
        public bool CheckAccessAuthorization(string key)
        {
            bool hasAuthorization = false;
            string pin = "ef258503-eb0f-4c4b-b265-99762f5d";

            hasAuthorization = pin.Equals(key) ? true : false;
            return hasAuthorization;
        }
    }
}
