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
            bool hasAuthorization = GetAuthorization();

            if (hasAuthorization)
            {
                TestBackend();
            }
            else
            {
                return;
            }
        }

        static bool GetAuthorization()
        {
            CheckAuthorization checkAuthorization = new CheckAuthorization();
            string key = "ef258503-eb0f-4c3b-b265-99762f5d";

            bool hasAuthorization = checkAuthorization.CheckAccessAuthorization(key);

            if (hasAuthorization)
                Debug.WriteLine("Has Authorization");
            else
                Debug.WriteLine("No Authorization");

            return hasAuthorization;
        }

        static void TestBackend()
        {
            // test backend
            CrimeAndArrestDataRetrieve crimeAndArrestDataRetrieve = new CrimeAndArrestDataRetrieve();
            AddressPurification addressPurification = new AddressPurification();
            VisualizationDataAggregation visualizationDataAggregation = new VisualizationDataAggregation();

        }
    }
}
