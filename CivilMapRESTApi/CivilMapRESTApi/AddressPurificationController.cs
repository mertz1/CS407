using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.Threading;
using CivilMapRESTApi.Models;
using CivilMapSQLDatabase;

namespace CivilMapRESTApi.Controllers
{
    public class AddressPurificationController : ApiController
    {
        [HttpGet]
        [Route("api/addressPurification/getCivilMapPurifiedAddress")]
        public HttpResponseMessage GetCivilMapPurifiedAddress()
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<PurifiedAddressRESTModel>();
                HttpResponseMessage response;
                try
                {
                    var list = addressPurification.GetCivilMapPurifiedAddress();

                    foreach (var item in list)
                    {
                        resultList.Add(ConvertPurifiedAddressModelToRESTModel(item));
                    }

                    if(resultList.Count() == 0)
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Not Found, please check URL");
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, resultList.AsEnumerable());
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }
        
        [HttpGet]
        [Route("api/addressPurification/selectCivilMapPurifiedAddress/{streetNumber}/{street}/{city}/{zipcode}")]
        public HttpResponseMessage SelectCivilMapPurifiedAddress(string streetNumber, string street, string city, string zipcode)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<PurifiedAddressRESTModel>();
                HttpResponseMessage response;
                try
                {
                    var model = new CivilMapSQLDatabaseConnection.PurifiedAddressModel
                    {
                        AddressModel = new CivilMapSQLDatabaseConnection.AddressModel
                        {
                            StreetNumber = streetNumber,
                            Street = street,
                            City = city,
                            Zipcode = zipcode
                        }
                    };
                    var list = addressPurification.SelectCivilMapPurifiedAddress(model);
                    foreach(var item in list)
                    {
                        resultList.Add(ConvertPurifiedAddressModelToRESTModel(item));
                    }

                    if (resultList.Count() == 0)
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Not Found, please check URL");
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, resultList.AsEnumerable());
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }


        [HttpPost]
        [Route("api/addressPurification/addCivilMapPurifiedAddress")]
        public HttpResponseMessage AddCivilMapPurifiedAddress([FromBody]PurifiedAddressRESTModel purifiedAddress)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                HttpResponseMessage response;
                try
                {
                    var item = ConvertPurifiedAddressRESTModelToDBModel(purifiedAddress);                    
                    addressPurification.AddCivilMapPurifiedAddress(item);
                    response = Request.CreateResponse(HttpStatusCode.Created, item);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }

        [HttpGet]
        [Route("api/addressPurification/getCivilMapPoints")]
        public HttpResponseMessage GetCivilMapPoints()
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<PointsRESTModel>();
                HttpResponseMessage response;
                try
                {
                    var list = addressPurification.GetCivilMapPoints();

                    foreach (var item in list)
                    {
                        resultList.Add(ConvertPointsModelToRESTModel(item));
                    }

                    if (resultList.Count() == 0)
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Not Found, please check URL");
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, resultList.AsEnumerable());
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }


        [HttpGet]
        [Route("api/addressPurification/selectCivilMapPoints/{pointId}")]
        public HttpResponseMessage SelectCivilMapPoints(string pointId)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<PointsRESTModel>();
                HttpResponseMessage response;
                try
                {
                    var model = new CivilMapSQLDatabaseConnection.PointsModel
                    {
                        PointId = Guid.Parse(pointId)
                    };

                    var list = addressPurification.SelectCivilMapPoints(model);

                    foreach (var item in list)
                    {
                        resultList.Add(ConvertPointsModelToRESTModel(item));
                    }

                    if (resultList.Count() == 0)
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Not Found, please check URL");
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, resultList.AsEnumerable());
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }

        [HttpPost]
        [Route("api/addressPurification/addCivilMapPoints")]
        public HttpResponseMessage AddCivilMapPoints([FromBody]PointsRESTModel points)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                HttpResponseMessage response;
                try
                {
                    var item = ConvertPointsRESTModelToDBModel(points);
                    addressPurification.AddCivilMapPoints(item);
                    response = Request.CreateResponse(HttpStatusCode.Created, item);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }


        [HttpGet]
        [Route("api/addressPurification/getCivilMapNonPurifiedAddress")]
        public HttpResponseMessage GetCivilMapNonPurifiedAddress()
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<NonPurifiedAddressRESTModel>();
                HttpResponseMessage response;
                try
                {
                    var list = addressPurification.GetCivilMapNonPurifiedAddress();
                    foreach(var item in list)
                    {
                        resultList.Add(ConvertNonPurifiedAddressModelToRESTModel(item));
                    }

                    if (resultList.Count() == 0)
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Not Found, please check URL");
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, resultList.AsEnumerable());
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }

        [HttpGet]
        [Route("api/addressPurification/selectCivilMapNonPurifiedAddress/{streetNumber}/{street}/{city}/{zipcode}")]
        public HttpResponseMessage SelectCivilMapNonPurifiedAddress(string streetNumber, string street, string city, string zipcode)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<NonPurifiedAddressRESTModel>();
                HttpResponseMessage response;
                try
                {
                    var model = new CivilMapSQLDatabaseConnection.NonPurifiedAddressModel
                    {
                        AddressModel = new CivilMapSQLDatabaseConnection.AddressModel
                        {
                            StreetNumber = streetNumber,
                            Street = street,
                            City = city,
                            Zipcode = zipcode
                        }
                    };
                    var list = addressPurification.SelectCivilMapNonPurifiedAddress(model);
                    foreach(var item in list)
                    {
                        resultList.Add(ConvertNonPurifiedAddressModelToRESTModel(item));
                    }

                    if (resultList.Count() == 0)
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Not Found, please check URL");
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, resultList.AsEnumerable());
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }

        
        [HttpGet]
        [Route("api/addressPurification/selectAliasNonPurifiedAddress/{purifiedAddressId}")]
        public HttpResponseMessage SelectAliasNonPurifiedAddress(string purifiedAddressId)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<NonPurifiedAddressRESTModel>();
                HttpResponseMessage response;
                try
                {
                    var id = Guid.Parse(purifiedAddressId);
                    var list = addressPurification.SelectAliasNonPurifiedAddress(id);
                    foreach (var item in list)
                    {
                        resultList.Add(ConvertNonPurifiedAddressModelToRESTModel(item));
                    }

                    if (resultList.Count() == 0)
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Not Found, please check URL");
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, resultList.AsEnumerable());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }


        [HttpPost]
        [Route("api/addressPurification/addCivilMapNonPurifiedAddress")]
        public HttpResponseMessage AddCivilMapNonPurifiedAddress([FromBody]NonPurifiedAddressRESTModel nonPurifiedAddress)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                HttpResponseMessage response;
                try
                {
                    var item = ConvertNonPurifiedAddressRESTModelToDBModel(nonPurifiedAddress);
                    addressPurification.AddCivilMapNonPurifiedAddress(item);
                    response = Request.CreateResponse(HttpStatusCode.Created, item);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }
        

        [HttpPost]
        [Route("api/addressPurification/addValidationCivilMapNonPurifiedAddress")]
        public HttpResponseMessage AddValidationCivilMapNonPurifiedAddress([FromBody]NonPurifiedAddressRESTModel nonPurifiedAddress)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                HttpResponseMessage response;
                try
                {
                    var item = ConvertNonPurifiedAddressRESTModelToDBModel(nonPurifiedAddress);
                    addressPurification.AddValidationCivilMapNonPurifiedAddress(item);
                    response = Request.CreateResponse(HttpStatusCode.Created, item);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }

        [HttpPut]
        [Route("api/addressPurification/updateCivilMapNonPurifiedAddress/{id}")]
        public HttpResponseMessage UpdateCivilMapNonPurifiedAddress(string id, [FromBody]NonPurifiedAddressRESTModel nonPurifiedAddress)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                HttpResponseMessage response;
                try
                {
                    if (nonPurifiedAddress == null || !nonPurifiedAddress.NonPurifiedAddressId.Equals(id))
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound, "Id does not match model identifier, please check URL");
                        return response;
                    } 
                    else
                    {
                        var item = ConvertNonPurifiedAddressRESTModelToDBModel(nonPurifiedAddress);
                        item.NonPurifiedAddressId = Guid.Parse(nonPurifiedAddress.NonPurifiedAddressId);
                        addressPurification.UpdateCivilMapNonPurifiedAddress(item);
                        response = Request.CreateResponse(HttpStatusCode.OK, item);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }


        [HttpGet]
        [Route("api/addressPurification/validateAddress/{streetNumber}/{street}/{city}/{zipcode}")]
        public HttpResponseMessage ValidateAddress(string streetNumber, string street, string city, string zipcode)
        {
            
            using (AddressPurification addressPurification = new AddressPurification())
            {
                HttpResponseMessage response;
                try
                {
                    var item = new CivilMapSQLDatabaseConnection.PurifiedAddressModel
                    {
                        AddressModel = new CivilMapSQLDatabaseConnection.AddressModel
                        {
                            StreetNumber = streetNumber,
                            Street = street,
                            City = city,
                            Zipcode = zipcode
                        }
                    };
                    addressPurification.ValidateAddress(item); 
                    response = Request.CreateResponse(HttpStatusCode.OK, item);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }

        [HttpGet]
        [Route("api/addressPurification/validate100Addresses")]
        public HttpResponseMessage Validate100Addresses()
        {
                        Console.WriteLine("API CALL");

            using (AddressPurification addressPurification = new AddressPurification())
            {
                HttpResponseMessage response;
                try
                {
                    addressPurification.Validate100Addresses();
                    response = Request.CreateResponse(HttpStatusCode.OK, item);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ex.Message);
                }
                return response;
            }
        }

        
        public AddressRESTModel ConvertAddressModelToRESTModel(CivilMapSQLDatabaseConnection.AddressModel addressModel)
        {
            AddressRESTModel addressRESTModel = new AddressRESTModel
            {
                StreetNumber = addressModel.StreetNumber,
                Street = addressModel.Street,
                City = addressModel.City,
                Zipcode = addressModel.Zipcode
            };
            return addressRESTModel;
        }

        public CivilMapSQLDatabaseConnection.AddressModel ConvertAddressRESTModelToDBModel(AddressRESTModel addressRESTModel)
        {
            CivilMapSQLDatabaseConnection.AddressModel addressModel = new CivilMapSQLDatabaseConnection.AddressModel
            {
                StreetNumber = addressRESTModel.StreetNumber,
                Street = addressRESTModel.Street,
                City = addressRESTModel.City,
                Zipcode = addressRESTModel.Zipcode
            };
            return addressModel;
        }

        public PurifiedAddressRESTModel ConvertPurifiedAddressModelToRESTModel(CivilMapSQLDatabaseConnection.PurifiedAddressModel purifiedAddress)
        {
            PurifiedAddressRESTModel purifiedAddressRESTModel = new PurifiedAddressRESTModel
            {
                PurifiedAddressId = purifiedAddress.PurifiedAddressId.ToString(),
                AddressRESTModel = ConvertAddressModelToRESTModel(purifiedAddress.AddressModel),
                Longitude = purifiedAddress.Longitude,
                Latitude = purifiedAddress.Latitude
            };
            return purifiedAddressRESTModel;
        }

        public CivilMapSQLDatabaseConnection.PurifiedAddressModel ConvertPurifiedAddressRESTModelToDBModel(PurifiedAddressRESTModel model)
        {
            CivilMapSQLDatabaseConnection.PurifiedAddressModel purifiedAddress = new CivilMapSQLDatabaseConnection.PurifiedAddressModel
            {
                AddressModel = ConvertAddressRESTModelToDBModel(model.AddressRESTModel),
                Longitude = model.Longitude,
                Latitude = model.Latitude
            };
            return purifiedAddress;
        }

        public NonPurifiedAddressRESTModel ConvertNonPurifiedAddressModelToRESTModel(CivilMapSQLDatabaseConnection.NonPurifiedAddressModel nonPurifiedAddressModel)
        {
            NonPurifiedAddressRESTModel nonPurifiedAddressRESTModel = new NonPurifiedAddressRESTModel
            {
                NonPurifiedAddressId = nonPurifiedAddressModel.NonPurifiedAddressId.ToString(),
                AddressRESTModel = ConvertAddressModelToRESTModel(nonPurifiedAddressModel.AddressModel),
                PurifiedAddressId = nonPurifiedAddressModel.PurifiedAddressId.ToString()
            };
            return nonPurifiedAddressRESTModel;
        }

        public CivilMapSQLDatabaseConnection.NonPurifiedAddressModel ConvertNonPurifiedAddressRESTModelToDBModel(NonPurifiedAddressRESTModel model)
        {
            CivilMapSQLDatabaseConnection.NonPurifiedAddressModel nonPurifiedAddress = new CivilMapSQLDatabaseConnection.NonPurifiedAddressModel
            {
                AddressModel = ConvertAddressRESTModelToDBModel(model.AddressRESTModel),
                PurifiedAddressId = (String.IsNullOrEmpty(model.PurifiedAddressId))? (Guid?)null : Guid.Parse(model.PurifiedAddressId)
            };
            return nonPurifiedAddress;
        }

        public PointsRESTModel ConvertPointsModelToRESTModel(CivilMapSQLDatabaseConnection.PointsModel pointsModel)
        {
            PointsRESTModel pointsRESTModel = new PointsRESTModel
            {
                PointId = pointsModel.PointId.ToString(),
                PurifiedAddressId = pointsModel.PurifiedAddressId.ToString()
            };
            return pointsRESTModel;
        }

        public CivilMapSQLDatabaseConnection.PointsModel ConvertPointsRESTModelToDBModel(PointsRESTModel model)
        {
            CivilMapSQLDatabaseConnection.PointsModel points = new CivilMapSQLDatabaseConnection.PointsModel
            {
                PurifiedAddressId = (String.IsNullOrEmpty(model.PurifiedAddressId)) ? (Guid?)null : Guid.Parse(model.PurifiedAddressId)
            };
            return points;
        } 
    }
}
