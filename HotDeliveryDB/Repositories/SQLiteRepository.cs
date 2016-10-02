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
        private DeliveriesContext _Context { get; }

        public SQLiteRepository(string connectionString)
        {
            _ConnectionString = connectionString;
            //using (DeliveriesContext context = new DeliveriesContext(_ConnectionString))
            //{
            //    context.Database.EnsureCreated();
            //}
            this._Context = new DeliveriesContext(_ConnectionString);
            _Context.Database.EnsureCreated();
        }


        public List<Delivery> GetDeliveryList()
        {
            //using (DeliveriesContext context = new DeliveriesContext(_ConnectionString))
            //{
            //var subset = context.Deliveries;
            //List<Delivery> deliveries = context.Deliveries.ToList();
            //return deliveries;
            //}
            var subset = _Context.Deliveries;
            List<Delivery> deliveries = _Context.Deliveries.ToList();
            return deliveries;

        }

        public Delivery GetDelivery(int id)
        {
            //using (DeliveriesContext context = new DeliveriesContext(_ConnectionString))
            //{
            //    var item = (from delivery in context.Deliveries
            //                where delivery.Id == id
            //                select new Delivery
            //                {
            //                    Id = delivery.Id,
            //                    Status = delivery.Status,
            //                    Title = delivery.Title,
            //                    UserId = delivery.UserId,
            //                    CreationTime = delivery.CreationTime,
            //                    ModificationTime = delivery.ModificationTime,
            //                    ExpirationTime = delivery.ExpirationTime
            //                }).FirstOrDefault();
            //    return item;
            //}

            var item = (from delivery in _Context.Deliveries
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

        public Delivery Create(Delivery item)
        {
            //using (DeliveriesContext context = new DeliveriesContext(_ConnectionString))
            //{
            //    context.Deliveries.Add(item);
            //    context.SaveChanges();
            //}
            _Context.Deliveries.Add(item);
            _Context.SaveChanges();
            return item;
        }

        public void Update(Delivery item)
        {

            //using (DeliveriesContext context = new DeliveriesContext(_ConnectionString))
            //{
            //    context.Deliveries.Update(item);
            //    context.SaveChanges();
            //}
            _Context.Deliveries.Update(item);
            _Context.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
