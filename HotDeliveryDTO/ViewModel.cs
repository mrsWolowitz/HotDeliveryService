using HotDeliveryDB;
using System;
using System.Collections.Generic;
using System.Linq;
using HotDeliveryDB.Types;

namespace HotDeliveryModel
{
    public class ViewModel:IHotDeliveryViewModel
    {
        private IRepository _DataBase;
        public ViewModel(string dBFormat, string dBPath)
        {
            if (dBFormat == "XMLFile")
                _DataBase = new XmlRepository(dBPath);
            else if (dBFormat == "SQLite")
            {
                _DataBase = new SQLiteRepository(dBPath);
            }
        }

        public ViewModel(IRepository repository)
        {
            _DataBase = repository;
        }

        public List<DeliveryDTO> GetAvailableDeliveries()
        {
            List<Delivery> deliveries = _DataBase.GetDeliveryList();
            var subset = deliveries.Where(i => i.Status == Status.Available).ToList();

            return subset.ConvertAll(DeliveryToDeliveryDTO);
        }

        public ResponseDTO TakeDelivery(int deliveryId, int userId)
        {
            Delivery delivery = _DataBase.GetDelivery(deliveryId);
            if (delivery == null)
                return new ResponseDTO(ResponseType.NotFound, "Доставка не найдена");
            if (delivery.Status != Status.Available)
                return new ResponseDTO(ResponseType.NotAvailable, "Статус доставки не Available");
            delivery.UserId = userId;
            delivery.Status = Status.Taken;
            delivery.ModificationTime = DateTime.Now;
            _DataBase.Update(delivery);
            return new ResponseDTO(ResponseType.Ok);
        }

        private static DeliveryDTO DeliveryToDeliveryDTO(Delivery delivery)
        {
            DeliveryDTO deliveryDto = new DeliveryDTO();
            deliveryDto.Id = delivery.Id;
            deliveryDto.CreationTime = delivery.CreationTime;
            deliveryDto.ExpirationTime = delivery.ExpirationTime;
            deliveryDto.ModificationTime = delivery.ModificationTime;
            deliveryDto.Status = delivery.Status;
            deliveryDto.Title = delivery.Title;
            deliveryDto.UserId = delivery.UserId;
            return deliveryDto;
        }
        
    }
}
