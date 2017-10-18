using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivilMapSQLDatabase;

namespace CivilMapTestApplication
{
    partial class TestApplication
    {
        static void Main(string[] args)
        {
            CheckAuthorization checkAuthorization = new CheckAuthorization();
            string key = "ef258503-eb0f-4c3b-b265-99762f5d";

            bool hasAuthorization = checkAuthorization.CheckAccessAuthorization(key);

            if (hasAuthorization)
            {
                Debug.WriteLine("Has Authorization");
            }
            else
            {
                Debug.WriteLine("No Authorization");
            }
        }
    }
}
