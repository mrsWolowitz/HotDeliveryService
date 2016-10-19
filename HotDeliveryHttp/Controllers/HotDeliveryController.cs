using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using HotDeliveryModel;
using System.Configuration;

namespace HotDeliveryHttp.Controllers
{
    public class HotDeliveryController : ApiController
    {
        private IHotDeliveryViewModel _ViewModel;

        public HotDeliveryController()
        {
            string dBFormat = ConfigurationManager.AppSettings["DBFormat"];
            string path = ConfigurationManager.ConnectionStrings[dBFormat].ConnectionString;

            _ViewModel = new ViewModel(dBFormat, path);
        }

        public HotDeliveryController(IHotDeliveryViewModel viewModel)
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
                return Content((HttpStatusCode) result.Type, result.Message);
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, "InternalServerError");
            }
        }
    }
}
