using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HotDeliveryDB;
using HotDeliveryDB.Types;
using System.Configuration;

namespace HotDeliveryHttp.Controllers
{
    public class ValuesController : ApiController
    {
        private IRepository db;

        public ValuesController()
        {
            AppSettings settings = new AppSettings();

            settings.ConnectionStrings.DBFormat = ConfigurationManager.AppSettings["DBFormat"];
            settings.ConnectionStrings.Path = ConfigurationManager.ConnectionStrings[settings.ConnectionStrings.DBFormat].ConnectionString;

            if (settings.ConnectionStrings.DBFormat == "XMLFile")
                db = new XmlRepository(settings.ConnectionStrings.Path);
            else if (settings.ConnectionStrings.DBFormat == "SQLite")
            {
                db = new SQLiteRepository(settings.ConnectionStrings.Path);
            }
        }

        public ValuesController(IRepository repository)
        {
            db = repository;
        }

        // GET api/values
        [HttpGet]
        public IHttpActionResult GetAvailableDeliveries()
        {
            try
            {
                List<Delivery> deliveries = db.GetDeliveryList();
                var subset = deliveries.Where(i => i.Status == Enum.GetName(typeof(Status), Status.Available));
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
                Delivery delivery = db.GetDelivery(deliveryId);
                if (delivery == null)
                    return Content(HttpStatusCode.NotFound, "Доставка не найдена");
                if (delivery.Status != Enum.GetName(typeof(Status), Status.Available))
                    return Content((HttpStatusCode)422, "Статус доставки не Available");
                delivery.UserId = userId;
                delivery.Status = Enum.GetName(typeof(Status), Status.Taken);
                delivery.ModificationTime = DateTime.Now;
                db.Update(delivery);
                return Ok();
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, "InternalServerError");
            }
        }
    }
}
