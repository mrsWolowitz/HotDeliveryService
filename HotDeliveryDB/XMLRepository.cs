using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HotDeliveryDB
{
    public class XmlRepository : IRepository
    {
        private string _ConnectionString { get; }
        private XDocument _Database;

        public XmlRepository(string connectionString)
        {
            _ConnectionString = connectionString;
            _Database = new XDocument();
            XElement deliveries = new XElement("Deliveries");
            _Database.Add(deliveries);
            //using (var stream = File.OpenWrite(_ConnectionString))
            //{
            //    _Database.Save(stream);
            //}
            _Database.Save(_ConnectionString);
        }

        public List<Delivery> GetDeliveryList()
        {
            var items = from xe in _Database.Element("Deliveries").Elements("Delivery")
                        select new Delivery
                        {
                            Id = Int32.Parse(xe.Attribute("Id").Value),
                            Status = xe.Element("Status").Value,
                            Title = xe.Element("Title").Value,
                            UserId = _ParseNullableInt(xe.Element("UserId").Value),
                            CreationTime = DateTime.Parse(xe.Element("CreationTime").Value),
                            ModificationTime = DateTime.Parse(xe.Element("ModificationTime").Value),
                            ExpirationTime = DateTime.Parse(xe.Element("ExpirationTime").Value)
                        };

            return items.ToList();
        }

        public Delivery GetDelivery(int id)
        {
            var items = from xe in _Database.Element("Deliveries").Elements("Delivery")
                        where Int32.Parse(xe.Attribute("Id").Value) == id
                        select new Delivery
                        {
                            Id = Int32.Parse(xe.Attribute("Id").Value),
                            Status = xe.Element("Status").Value,
                            Title = xe.Element("Title").Value,
                            UserId = _ParseNullableInt(xe.Element("UserId").Value),
                            CreationTime = DateTime.Parse(xe.Element("CreationTime").Value),
                            ModificationTime = DateTime.Parse(xe.Element("ModificationTime").Value),
                            ExpirationTime = DateTime.Parse(xe.Element("ExpirationTime").Value)
                        };
            return items.FirstOrDefault();
        }

        public Delivery Create(Delivery item)
        {
            XElement root = _Database.Element("Deliveries");
            int maxId;
            if (root.Elements("Delivery").Any())
            {
                maxId = root.Elements("Delivery").Max(t => Int32.Parse(t.Attribute("Id").Value));
            }

            else
                maxId = -1;
            item.Id = ++maxId;
            DateTime currentTime = DateTime.Now;
            XElement delivery = new XElement("Delivery",
                new XAttribute("Id", item.Id),
                new XElement("Status", item.Status),
                new XElement("Title", item.Title),
                new XElement("UserId", item.UserId),
                new XElement("CreationTime", currentTime.ToString()),
                new XElement("ModificationTime", currentTime.ToString()),
                new XElement("ExpirationTime", item.ExpirationTime));
            root.Add(delivery);
            //using (var stream = File.OpenWrite(_ConnectionString))
            //{
            //    _Database.Save(stream);
            //}
            _Database.Save(_ConnectionString);
            return item;
        }

        public void Update(Delivery item)
        {
            XElement root = _Database.Element("Deliveries");
            foreach (var delivery in root.Elements("Delivery").ToList())
            {
                if (Int32.Parse(delivery.Attribute("Id").Value) == item.Id)
                {
                    delivery.Element("Status").Value = item.Status;
                    delivery.Element("Title").Value = item.Title;
                    delivery.Element("UserId").Value = item.UserId.ToString();
                    delivery.Element("CreationTime").Value = item.CreationTime.ToString();
                    DateTime currentTime = DateTime.Now;
                    delivery.Element("ModificationTime").Value = currentTime.ToString();
                    delivery.Element("ExpirationTime").Value = item.ExpirationTime.ToString();
                }
            }
            //using (var stream = File.OpenWrite(_ConnectionString))
            //{
            //    _Database.Save(stream);
            //}
            _Database.Save(_ConnectionString);
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        private int? _ParseNullableInt(string val)
        {
            int i;
            return int.TryParse(val, out i) ? (int?)i : null;
        }

    }
}
