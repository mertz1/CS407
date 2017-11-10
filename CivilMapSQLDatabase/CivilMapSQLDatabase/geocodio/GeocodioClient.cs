using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using CivilMapSQLDatabase;
using System.IO;



namespace GeocodioClient
{
    public class GeocodioClient
    {
        public string ApiKey { get; private set; }
        private RestClient client;
        private static readonly HttpClient clientA = new HttpClient();

        /// <summary>
        /// Creates a new geocoding client, which can convert street addresses into latitude and longitude coordinates.
        /// </summary>
        /// <param name="apiKey">Your API Key. You can request one for free at http://geocod.io/ .</param>
        public GeocodioClient(string apiKey)
        {
            ApiKey = apiKey;
            client = new RestClient("http://api.geocod.io/v1/");
        }

        /// <summary>
        /// Gets a result from Geocodio
        /// </summary>
        /// <param name="address">A pre-concatentated street address. Ex: "42370 Bob Hope Dr, Rancho Mirage, CA".</param>
        /// <returns></returns>
        public CivilMapSQLDatabaseConnection.PurifiedAddressModel Geocode(string address)
        {
            var request = new RestRequest("geocode", Method.GET);
            request.AddParameter("q", address);
            return PerformGeocoding(request);
        }

        /// <summary>
        /// Gets a result from Geocodio
        /// </summary>
        /// <param name="street">Contains the house / unit number and the street name. Ex: "42370 Bob Hope Dr".</param>
        /// <param name="postalCode">ZIP code</param>
        /// <param name="concatenateInput">Sometimes Geocodio provides better results when the address parts are concatenated before sending them in</param>
        public CivilMapSQLDatabaseConnection.PurifiedAddressModel Geocode(string street, string city, string state = null, string postalCode = null, bool concatenateInput = false)
        {
            var request = new RestRequest("geocode", Method.GET);

            if (!concatenateInput)
            {
                request.AddParameter("street", street);
                request.AddParameter("city", city);

                if (!string.IsNullOrWhiteSpace(state))
                    request.AddParameter("state", state);

                if (!string.IsNullOrWhiteSpace(postalCode))
                    request.AddParameter("postal_code", postalCode);
            }
            else
            {
                StringBuilder addressString = new StringBuilder(street + ", " + city);

                if (!string.IsNullOrWhiteSpace(state))
                    addressString.Append(", " + state);

                if (!string.IsNullOrWhiteSpace(postalCode))
                    addressString.Append(", " + postalCode);

                request.AddParameter("q", addressString.ToString());
            }

            Debug.WriteLine("PerformGeocoding");

            return PerformGeocoding(request);
        }

        /// <summary>
        /// Gets a result from Geocodio
        /// </summary>
        /// <param name="street">Contains the house / unit number and the street name. Ex: "42370 Bob Hope Dr".</param>
        /// <param name="postalCode">ZIP code</param>
        /// <param name="concatenateInput">Sometimes Geocodio provides better results when the address parts are concatenated before sending them in</param>
        public List<CivilMapSQLDatabaseConnection.PurifiedAddressModel?> BatchGeocode(List<string> addresses)
        {
            var client = new RestClient("https://api.geocod.io/v1/geocode?api_key=3551a95da75a999c89a259153a77d2aa9a3d5a2");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");

            var jsonAddresses = Newtonsoft.Json.JsonConvert.SerializeObject(addresses);

            request.AddParameter("application/json", jsonAddresses, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            List<CivilMapSQLDatabaseConnection.PurifiedAddressModel?> validatedAddresses = new List<CivilMapSQLDatabaseConnection.PurifiedAddressModel?>();

            BatchGeocodioResultArray list = Newtonsoft.Json.JsonConvert.DeserializeObject<BatchGeocodioResultArray>(response.Content);

            var count = list.array.Count;




            for (int i = 0; i < list.array.Count; i++)
            {
                var addressComponent = list.array[i].response.results[0].address_components;
                var locationComponent = list.array[i].response.results[0].location;

                if (addressComponent.street == null || addressComponent.number == null || addressComponent.city == null || addressComponent.zip == null)
                {
                    validatedAddresses.Add(null);
                }
                else
                {
                    CivilMapSQLDatabaseConnection.PurifiedAddressModel validatedAddress = new CivilMapSQLDatabaseConnection.PurifiedAddressModel()
                    {
                        AddressModel = new CivilMapSQLDatabaseConnection.AddressModel
                        {
                            Street = addressComponent.street,
                            StreetNumber = addressComponent.number,
                            City = addressComponent.city,
                            Zipcode = addressComponent.zip
                        },
                        Latitude = Convert.ToDecimal(locationComponent.lat),
                        Longitude = Convert.ToDecimal(locationComponent.lng)
                    };

                    validatedAddresses.Add(validatedAddress);
                }
            }



            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        return validatedAddresses;
                    }
                case HttpStatusCode.Forbidden:
                    {
                        throw new ArgumentException("Your API key was rejected by Geocodio\n\nError Description: " + response.StatusDescription);
                    }
                case (HttpStatusCode)422:
                    {
                        // HTTP 422: Unprocessable Entry
                        throw new FormatException("Your address was badly formed: " + response.StatusDescription);
                    }
                default:
                    {
                        throw new InvalidOperationException("Geocoding failed!\n\nHTTP status code:" + response.StatusCode + "\nError Description:" + response.StatusDescription);
                    }
            }

        }

        private CivilMapSQLDatabaseConnection.PurifiedAddressModel PerformGeocoding(RestRequest request)
        {
            request.AddParameter("api_key", ApiKey);

            var response = client.Execute<GeocodioResponse>(request);

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(response))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(response);
                Debug.WriteLine("{0}={1}", name, value);
            }

            var parsedResponse = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
            var addressComponent = parsedResponse["results"][0]["address_components"];
            var locationComponent = parsedResponse["results"][0]["location"];


            CivilMapSQLDatabaseConnection.PurifiedAddressModel validatedAddress = new CivilMapSQLDatabaseConnection.PurifiedAddressModel()
            {
                AddressModel = new CivilMapSQLDatabaseConnection.AddressModel
                {
                    Street = addressComponent["formatted_street"].ToString(),
                    StreetNumber = addressComponent["number"].ToString(),
                    City = addressComponent["city"].ToString(),
                    Zipcode = addressComponent["zip"].ToString()
                },
                Latitude = Convert.ToDecimal(locationComponent["lat"]),
                Longitude = Convert.ToDecimal(locationComponent["lng"])
            };

            Debug.WriteLine("Response 1: " + response.StatusCode);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        return validatedAddress;
                    }
                case HttpStatusCode.Forbidden:
                    {
                        throw new ArgumentException("Your API key was rejected by Geocodio\n\nError Description: " + response.StatusDescription);
                    }
                case (HttpStatusCode)422:
                    {
                        // HTTP 422: Unprocessable Entry
                        throw new FormatException("Your address was badly formed: " + response.StatusDescription);
                    }
                default:
                    {
                        throw new InvalidOperationException("Geocoding failed!\n\nHTTP status code:" + response.StatusCode + "\nError Description:" + response.StatusDescription);
                    }
            }
        }
    }
}