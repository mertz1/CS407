using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilMapTestApplication
{
    class Authorization
    {
        public bool CheckAccessAuthorization(string key)
        {
            bool hasAuthorization = false;
            string pin = "ef258503-eb0f-4c4b-b265-99762f5d";

            hasAuthorization = pin.Equals(key) ? true : false;

            if (hasAuthorization)
                Debug.WriteLine("Has Authorization");
            else
                Debug.WriteLine("No Authorization");

            return hasAuthorization;
        }
    }
}
