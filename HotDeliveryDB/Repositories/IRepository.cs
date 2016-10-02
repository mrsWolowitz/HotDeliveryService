using System;
using System.Collections.Generic;

namespace HotDeliveryDB
{
    public interface IRepository: IDisposable
    {
        List<Delivery> GetDeliveryList();
        Delivery GetDelivery(int id);
        Delivery Create(Delivery item);
        void Update(Delivery item);
        void Delete(int id);
    }
}
