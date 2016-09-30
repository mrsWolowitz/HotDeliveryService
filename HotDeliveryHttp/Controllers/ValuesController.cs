using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HotDeliveryDB;
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

            settings.DeliveriesCountMin = Int32.Parse(ConfigurationManager.AppSettings["DeliveriesCountMin"]);
            settings.DeliveriesCountMax = Int32.Parse(ConfigurationManager.AppSettings["DeliveriesCountMax"]);
            settings.TaskIntervalMin = Int32.Parse(ConfigurationManager.AppSettings["TaskIntervalMin"]);
            settings.TaskIntervalMax = Int32.Parse(ConfigurationManager.AppSettings["TaskIntervalMax"]);
            settings.ExpirationTime = Int32.Parse(ConfigurationManager.AppSettings["ExpirationTime"]);

            if (settings.ConnectionStrings.DBFormat == "XMLFile")
                db = new XmlRepository(settings.ConnectionStrings.Path);
            else if (settings.ConnectionStrings.DBFormat == "SQLite")
            {
                db = new SQLiteRepository(settings.ConnectionStrings.Path);
            }
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Delivery> GetAvailableDeliveries()
        {
            List<Delivery> deliveries = db.GetDeliveryList();
            var subset = deliveries.Where(i => i.Status == "Available");
            return subset;
        }

        // PUT api/values/5
        public void TakeDelivery(int deliveryId, [FromBody]int userId)
        {
            Delivery delivery = db.GetDelivery(deliveryId);
            delivery.UserId = userId;
            delivery.Status = "Taken";
            db.Update(delivery);
        }

    }
}
