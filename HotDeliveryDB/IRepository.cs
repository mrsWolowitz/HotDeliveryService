using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotDeliveryDB
{
    public interface IRepository
    {
        List<Delivery> GetDeliveryList();
        Delivery GetDelivery(int id);
        Delivery Create(Delivery item);
        void Update(Delivery item);
        void Delete(int id);
    }
}
