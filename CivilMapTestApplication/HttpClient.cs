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
        // Purified Address
        static async Task<List<PurifiedAddressRESTModel>> GetPurifiedAddressAsync(string path)
        {
            List<PurifiedAddressRESTModel> purifiedAddress = null;
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
                purifiedAddress = await response.Content.ReadAsAsync<List<PurifiedAddressRESTModel>>();

            return purifiedAddress;
        }

        static async Task<List<PurifiedAddressRESTModel>> SelectPurifiedAddressAsync(string streetNumber, string direction, string street, string city, string zipcode)
        {
            List<PurifiedAddressRESTModel> purifiedAddress = null;
            HttpResponseMessage response = await client.GetAsync($"api/addressPurification/selectCivilMapPurifiedAddress/{streetNumber}/{direction}/{street}/{city}/{zipcode}");

            if (response.IsSuccessStatusCode)
                purifiedAddress = await response.Content.ReadAsAsync<List<PurifiedAddressRESTModel>>();

            return purifiedAddress;
        }

        static async Task<Uri> AddPurifiedAddressAsync(PurifiedAddressRESTModel model)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/addressPurification/addCivilMapPurifiedAddress", model);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }

        // Points
        static async Task<List<PointsRESTModel>> GetPointsAsync(string path)
        {
            List<PointsRESTModel> points = null;
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
                points = await response.Content.ReadAsAsync<List<PointsRESTModel>>();

            return points;
        }

        static async Task<List<PointsRESTModel>> SelectPointsAsync(string pointId)
        {
            List<PointsRESTModel> points = null;
            HttpResponseMessage response = await client.GetAsync($"api/addressPurification/selectCivilMapPoints/{pointId}");

            if (response.IsSuccessStatusCode)
                points = await response.Content.ReadAsAsync<List<PointsRESTModel>>();

            return points;
        }

        static async Task<Uri> AddPointsAsync(PointsRESTModel model)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/addressPurification/addCivilMapPoints", model);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }

        // Non Purified Address
        static async Task<List<NonPurifiedAddressRESTModel>> GetNonPurifiedAddressAsync(string path)
        {
            List<NonPurifiedAddressRESTModel> points = null;
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
                points = await response.Content.ReadAsAsync<List<NonPurifiedAddressRESTModel>>();

            return points;
        }

        static async Task<List<NonPurifiedAddressRESTModel>> SelectNonPurifiedAddressAsync(string streetNumber, string street, string city, string zipcode)
        {
            List<NonPurifiedAddressRESTModel> nonPurifiedAddress = null;
            HttpResponseMessage response = await client.GetAsync($"api/addressPurification/selectCivilMapNonPurifiedAddress/{streetNumber}/{street}/{city}/{zipcode}");

            if (response.IsSuccessStatusCode)
                nonPurifiedAddress = await response.Content.ReadAsAsync<List<NonPurifiedAddressRESTModel>>();

            return nonPurifiedAddress;
        }

        static async Task<List<NonPurifiedAddressRESTModel>> SelectAliasNonPurifiedAddressAsync(string purifiedAddressId)
        {
            List<NonPurifiedAddressRESTModel> nonPurifiedAddress = null;
            HttpResponseMessage response = await client.GetAsync($"api/addressPurification/selectAliasNonPurifiedAddress/{purifiedAddressId}");

            if (response.IsSuccessStatusCode)
                nonPurifiedAddress = await response.Content.ReadAsAsync<List<NonPurifiedAddressRESTModel>>();

            return nonPurifiedAddress;
        }

        static async Task<Uri> AddNonPurifiedAddressAsync(NonPurifiedAddressRESTModel model)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/addressPurification/addCivilMapNonPurifiedAddress", model);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }
        
        static async Task<NonPurifiedAddressRESTModel> UpdateNonPurifiedAddressAsync(string id, NonPurifiedAddressRESTModel model)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/addressPurification/updateCivilMapNonPurifiedAddress/{id}", model);
            response.EnsureSuccessStatusCode();

            model = await response.Content.ReadAsAsync<NonPurifiedAddressRESTModel>();
            return model;
        }

        static async Task<HttpStatusCode> ValidateAddressAsync(string streetNumber, string street, string city, string zipcode)
        {
            HttpResponseMessage response = await client.GetAsync($"api/addressPurification/validateAddress/{streetNumber}/{street}/{city}/{zipcode}");
            return response.StatusCode;
        }

        static async Task<HttpStatusCode> Validate100AddressesAsync()
        {
            Console.WriteLine("Async Call");
            HttpResponseMessage response = await client.GetAsync($"api/addressPurification/validate100Addresses");
            return response.StatusCode;
        }
    }
}
