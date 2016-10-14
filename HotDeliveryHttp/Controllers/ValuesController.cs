using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using HotDeliveryDB;
//using HotDeliveryDB.Types;
using HotDeliveryModel;
using System.Configuration;
using HotDeliveryDTO;

namespace HotDeliveryHttp.Controllers
{
    public class ValuesController : ApiController
    {
        private ViewModel _ViewModel;

        public ValuesController()
        {
            string dBFormat = ConfigurationManager.AppSettings["DBFormat"];
            string path = ConfigurationManager.ConnectionStrings[dBFormat].ConnectionString;

            _ViewModel = new ViewModel(dBFormat, path);
        }

        public ValuesController(ViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        // GET api/values
        [HttpGet]
        public IHttpActionResult GetAvailableDeliveries()
        {
            try
            {
                List<DeliveryDTO> subset = _ViewModel.GetAvailableDeliveries();
                return Ok(subset);
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, "InternalServerError");
            }
        }

        // PUT api/values/5/55
        [HttpPut]
        [Route("api/values/{deliveryId:int}/{userId:int}")]
        public IHttpActionResult TakeDelivery(int deliveryId, int userId)
        {
            try
            {
                ResponseDTO result = _ViewModel.TakeDelivery(deliveryId, userId);
                return Content((HttpStatusCode)result.Type, result.Message);
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, "InternalServerError");
            }
        }
    }
}
