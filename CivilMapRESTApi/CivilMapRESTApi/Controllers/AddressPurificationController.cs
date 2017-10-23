using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using CivilMapRESTApi.Models;
using CivilMapSQLDatabase;

namespace CivilMapRESTApi.Controllers
{
    public class AddressPurificationController : ApiController
    {
        [HttpGet]
        [Route("api/addressPurification/getCivilMapPurifiedAddress")]
        public IEnumerable<PurifiedAddressRESTModel> GetCivilMapPurifiedAddress()
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<PurifiedAddressRESTModel>();
                try
                {
                    var list = addressPurification.GetCivilMapPurifiedAddress();

                    foreach (var item in list)
                    {
                        resultList.Add(ConvertPurifiedAddressModelToRESTModel(item));
                    }
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }

                return resultList;
            }
        }
        
        [HttpGet]
        [Route("api/addressPurification/selectCivilMapPurifiedAddress/{streetNumber}/{street}/{city}/{zipcode}")]
        public List<PurifiedAddressRESTModel> SelectCivilMapPurifiedAddress(string streetNumber, string street, string city, string zipcode)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<PurifiedAddressRESTModel>();
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
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return resultList;
            }
        }


        [HttpPost]
        [Route("api/addressPurification/addCivilMapPurifiedAddress")]
        public void AddCivilMapPurifiedAddress([FromBody]PurifiedAddressRESTModel purifiedAddress)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                try
                {
                    var item = ConvertPurifiedAddressRESTModelToDBModel(purifiedAddress);                    
                    addressPurification.AddCivilMapPurifiedAddress(item);
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("api/addressPurification/getCivilMapPoints")]
        public IEnumerable<PointsRESTModel> GetCivilMapPoints()
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<PointsRESTModel>();
                try
                {
                    var list = addressPurification.GetCivilMapPoints();

                    foreach (var item in list)
                    {
                        resultList.Add(ConvertPointsModelToRESTModel(item));
                    }
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return resultList;
            }
        }

        [HttpGet]
        [Route("api/addressPurification/selectCivilMapPoints/{pointId}")]
        public List<PointsRESTModel> SelectCivilMapPoints(string pointId)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<PointsRESTModel>();
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
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return resultList;
            }
        }

        [HttpPost]
        [Route("api/addressPurification/addCivilMapPoints")]
        public void AddCivilMapPoints([FromBody]PointsRESTModel points)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                try
                {
                    var item = ConvertPointsRESTModelToDBModel(points);
                    addressPurification.AddCivilMapPoints(item);
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("api/addressPurification/getCivilMapNonPurifiedAddress")]
        public IEnumerable<NonPurifiedAddressRESTModel> GetCivilMapNonPurifiedAddress()
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<NonPurifiedAddressRESTModel>();
                try
                {
                    var list = addressPurification.GetCivilMapNonPurifiedAddress();
                    foreach(var item in list)
                    {
                        resultList.Add(ConvertNonPurifiedAddressModelToRESTModel(item));
                    }
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return resultList;
            }
        }

        [HttpGet]
        [Route("api/addressPurification/selectCivilMapNonPurifiedAddress/{streetNumber}/{street}/{city}/{zipcode}")]
        public List<NonPurifiedAddressRESTModel> SelectCivilMapNonPurifiedAddress(string streetNumber, string street, string city, string zipcode)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<NonPurifiedAddressRESTModel>();
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
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return resultList;
            }
        }

        
        [HttpGet]
        [Route("api/addressPurification/selectAliasNonPurifiedAddress/{purifiedAddressId}")]
        public List<NonPurifiedAddressRESTModel> SelectAliasNonPurifiedAddress(string purifiedAddressId)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                var resultList = new List<NonPurifiedAddressRESTModel>();
                try
                {
                    var id = Guid.Parse(purifiedAddressId);
                    var list = addressPurification.SelectAliasNonPurifiedAddress(id);
                    foreach (var item in list)
                    {
                        resultList.Add(ConvertNonPurifiedAddressModelToRESTModel(item));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
                return resultList;
            }
        }


        [HttpPost]
        [Route("api/addressPurification/addCivilMapNonPurifiedAddress")]
        public void AddCivilMapNonPurifiedAddress([FromBody]NonPurifiedAddressRESTModel nonPurifiedAddress)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                try
                {
                    var item = ConvertNonPurifiedAddressRESTModelToDBModel(nonPurifiedAddress);
                    addressPurification.AddCivilMapNonPurifiedAddress(item);
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }
        

        [HttpPost]
        [Route("api/addressPurification/addValidationCivilMapNonPurifiedAddress")]
        public void AddValidationCivilMapNonPurifiedAddress([FromBody]NonPurifiedAddressRESTModel nonPurifiedAddress)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                try
                {
                    var item = ConvertNonPurifiedAddressRESTModelToDBModel(nonPurifiedAddress);
                    addressPurification.AddValidationCivilMapNonPurifiedAddress(item);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        [HttpPut]
        [Route("api/addressPurification/updateCivilMapNonPurifiedAddress/{id}")]
        public void UpdateCivilMapNonPurifiedAddress(string id, [FromBody]NonPurifiedAddressRESTModel nonPurifiedAddress)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
                try
                {
                    if (nonPurifiedAddress == null || !nonPurifiedAddress.NonPurifiedAddressId.Equals(id))
                    {
                        return;
                    } 
                    else
                    {
                        var item = ConvertNonPurifiedAddressRESTModelToDBModel(nonPurifiedAddress);
                        item.NonPurifiedAddressId = Guid.Parse(nonPurifiedAddress.NonPurifiedAddressId);
                        addressPurification.UpdateCivilMapNonPurifiedAddress(item);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("api/addressPurification/ValidateAddress/{streetNumber}/{street}/{city}/{zipcode}")]
        public void ValidateAddress(string streetNumber, string street, string city, string zipcode)
        {
            using (AddressPurification addressPurification = new AddressPurification())
            {
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
                    addressPurification.ValidateAddress(model);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                }
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
