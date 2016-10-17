using HotDeliveryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotDeliveryModel
{
    public interface IHotDeliveryViewModel
    {
        List<DeliveryDTO> GetAvailableDeliveries();
        ResponseDTO TakeDelivery(int deliveryId, int userId);
    }
}
