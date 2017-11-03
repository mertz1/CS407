using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CivilMapTestApplication
{
    partial class TestApplication
    {
        static HttpClient client = new HttpClient();
        static string key = "ef258503-eb0f-4c4b-b265-99762f5d";
        static Authorization authorization = new Authorization();

        static void Main(string[] args)
        {
            bool hasAuthorization = authorization.CheckAccessAuthorization(key);

            if (hasAuthorization)
                RunAsync().Wait();
            else
                return;
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:51543/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                int task = 0;

                switch (task)
                {
                    case 0:
                        // get Purified Address
                        var getPurifiedAddress = await GetPurifiedAddressAsync("api/addressPurification/getCivilMapPurifiedAddress");
                        Debug.WriteLine("Get purified addresses #: " + getPurifiedAddress.Count());
                        break;

                    case 1:
                        // select Purified Address
                        var selectPurifiedAddress = await SelectPurifiedAddressAsync("42", "HICKS underground", "Chicago", "60412");
                        Debug.WriteLine("Select purified address #: " + selectPurifiedAddress.Count());
                        break;

                    case 2:
                        // add Purified Address
                        PurifiedAddressRESTModel model = CreatePurifiedAddressRESTModel();
                        var addPurifiedAddress = await AddPurifiedAddressAsync(model);
                        Debug.WriteLine("Add purified address: " + addPurifiedAddress);
                        break;

                    case 3:
                        // get Points
                        var getPoints = await GetPointsAsync("api/addressPurification/getCivilMapPoints");
                        Debug.WriteLine("Get points #: " + getPoints.Count());
                        break;

                    case 4:
                        // select Points
                        var selectPoints = await SelectPointsAsync("b1724a62-a38f-4c62-aaff-8e6bfcc940d3");
                        Debug.WriteLine("Select points #: " + selectPoints.Count());
                        break;

                    case 5:
                        // add Points
                        PointsRESTModel points = CreatePointsRESTModel();
                        var addPoints = await AddPointsAsync(points);
                        Debug.WriteLine("Add points: " + addPoints);
                        break;

                    case 6:
                        // get Non Purified Address
                        var getNonPurifiedAddress = await GetNonPurifiedAddressAsync("api/addressPurification/getCivilMapNonPurifiedAddress");
                        Debug.WriteLine("Get non purified address #: " + getNonPurifiedAddress.Count());
                        break;

                    case 7:
                        // select Non Purified Address:
                        var selectNonPurifiedAddress = await SelectNonPurifiedAddressAsync("350", "South Michigan Avenue", "Chicago", "60606");
                        Debug.WriteLine("Select non purified address #: " + selectNonPurifiedAddress.Count());
                        break;

                    case 8:
                        // select Alias Non Purified Address:
                        var selectAlias = await SelectAliasNonPurifiedAddressAsync("3bec84e5-9683-4d27-94e3-102998572a7d");
                        Debug.WriteLine("Select alias non purified address #: " + selectAlias.Count());
                        break;

                    case 9:
                        // add Non Purified Address:
                        NonPurifiedAddressRESTModel nonPurifiedAddress = CreateNonPurifiedAddressRESTModel();
                        var addNonPurifiedAddress = await AddNonPurifiedAddressAsync(nonPurifiedAddress);
                        Debug.WriteLine("Add non purified address: " + addNonPurifiedAddress);
                        break;

                    case 10:
                        // add Validation Non Purified Address:
                        NonPurifiedAddressRESTModel validateNonPurifiedAddress = CreateNonPurifiedAddressRESTModel();
                        var addValidate = await AddValidationNonPurifiedAddressAsync(validateNonPurifiedAddress);
                        Debug.WriteLine("Add validate non purified address: " + addValidate);
                        break;

                    case 11:
                        // Update non purified address:
                        NonPurifiedAddressRESTModel update = CreateNonPurifiedAddressForUpdate();
                        var updateNonPurifiedAddress = await UpdateNonPurifiedAddressAsync("18094643-16dc-4a48-b661-4e72f5ed8149", update);
                        Debug.WriteLine("Update non purified address: " + updateNonPurifiedAddress);
                        break;

                    case 12:
                        // Validate address:
                        var validate = await ValidateAddressAsync("189", "Russel St", "Chicago", "60412");
                        Debug.WriteLine("Validate address: " + validate);
                        break;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            Console.ReadLine();
        }
    }
}
