using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotDeliveryDB
{
    public class SQLiteRepository : IRepository
    {
        private string _ConnectionString { get; }
        private DeliveriesContext Context { get; }


        public SQLiteRepository(string connectionString)
        {
            _ConnectionString = connectionString;
            using (Context = new DeliveriesContext(connectionString))
            {
                Context.Database.EnsureCreated();
            }
        }


        public List<Delivery> GetDeliveryList()
        {
            using (DeliveriesContext context = new DeliveriesContext(_ConnectionString))
            {
                var subset = context.Deliveries;
                List<Delivery> deliveries = context.Deliveries.ToList();
                return deliveries;
            }

        }

        public Delivery GetDelivery(int id)
        {
            using (DeliveriesContext context = new DeliveriesContext(_ConnectionString))
            {
                var item = (from delivery in context.Deliveries
                            where delivery.Id == id
                            select new Delivery
                            {
                                Id = delivery.Id,
                                Status = delivery.Status,
                                Title = delivery.Title,
                                UserId = delivery.UserId,
                                CreationTime = delivery.CreationTime,
                                ModificationTime = delivery.ModificationTime,
                                ExpirationTime = delivery.ExpirationTime
                            }).FirstOrDefault();
                return item;
            }
        }

        public void Create(Delivery item)
        {
            using (DeliveriesContext context = new DeliveriesContext(_ConnectionString))
            {
                context.Deliveries.Add(item);
                context.SaveChanges();
            }
        }

        public void Update(Delivery item)
        {

            using (DeliveriesContext context = new DeliveriesContext(_ConnectionString))
            {
                context.Deliveries.Update(item);
                context.SaveChanges();
            }

        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
