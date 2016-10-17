using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotDeliveryDB;
using HotDeliveryDB.Types;
using Moq;

namespace HotDeliveryModel.Test
{
    [TestClass]
    public class HotdDeliveryModelTest
    {
        private IRepository _CreateMockRepository()
        {
            DateTime dateTime = DateTime.Now.AddHours(-1);
            var deliveries = new List<Delivery> {
        new Delivery {
            Id =1,
            CreationTime = dateTime,
            Status = Status.Expired,
            Title = "1111111",
            ModificationTime = dateTime.AddMinutes(1),
            ExpirationTime = dateTime.AddSeconds(5) },
        new Delivery {
            Id =2,
            CreationTime = dateTime,
            Status = Status.Available,
            Title = "2222222",
            ModificationTime = dateTime,
            ExpirationTime = dateTime.AddSeconds(5)},
            };

            Mock<IRepository> mock = new Mock<IRepository>();

            mock.Setup(m => m.GetDeliveryList()).Returns(deliveries);

            mock.Setup(m => m.Create(It.IsAny<Delivery>()))
                .Returns<Delivery>(c =>
                {
                    c.Id = deliveries.Count + 1;
                    c.CreationTime = dateTime;
                    deliveries.Add(c);
                    return c;
                });

            mock.Setup(m => m.GetDelivery(It.IsAny<int>()))
                .Returns<int>(c => deliveries.Where(q => q.Id == c).FirstOrDefault());

            mock.Setup(m=>m.Update(It.IsAny<Delivery>()))
                .Callback<Delivery>(u=> {
                    var original = deliveries.Where(q => q.Id == u.Id).Single();
                    original.Id = u.Id;
                }).Verifiable();

            return mock.Object;
        }


        [TestMethod]
        public void GetAvailableDeliveries_Ok()
        {
            // Arrange
            var mockRepository = _CreateMockRepository();
            ViewModel model = new ViewModel(mockRepository);

            // Act
            List<DeliveryDTO> result = model.GetAvailableDeliveries();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void TakeDelivery_Ok()
        {
            // Arrange
            var mockRepository = _CreateMockRepository();
            ViewModel model = new ViewModel(mockRepository);
            
            int id = 2;
            ResponseType expectedResponse = ResponseType.Ok;

            // Act
            ResponseDTO result = model.TakeDelivery(id, 55);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse, result.Type);
        }

        [TestMethod]
        public void TakeDelivery_SetStatusTaken()
        {
            // Arrange
            var mockRepository = _CreateMockRepository();
            ViewModel model = new ViewModel(mockRepository);
            
            int id = 2;
            string expectedStatus = Status.Taken;

            // Act
            ResponseDTO result = model.TakeDelivery(id, 55);

            // Assert
            Assert.AreEqual(expectedStatus, mockRepository.GetDelivery(id).Status);
        }

        [TestMethod]
        public void TakeDelivery_NotFound()
        {
            // Arrange
            var mockRepository = _CreateMockRepository();
            ViewModel model = new ViewModel(mockRepository);

            int id = 7;
            ResponseType expectedResponseType = ResponseType.NotFound;
            string expectedResponseMessage = "Доставка не найдена";

            // Act
            ResponseDTO result = model.TakeDelivery(id, 55);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponseType, result.Type);
            Assert.AreEqual(expectedResponseMessage, result.Message);
        }

        [TestMethod]
        public void TakeDelivery_NotAvailable()
        {
            // Arrange
            var mockRepository = _CreateMockRepository();
            ViewModel model = new ViewModel(mockRepository);

            int id = 1;
            ResponseType expectedResponseType = ResponseType.NotAvailable;
            string expectedResponseMessage = "Статус доставки не Available";

            // Act
            ResponseDTO result = model.TakeDelivery(id, 55);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponseType, result.Type);
            Assert.AreEqual(expectedResponseMessage, result.Message);
        }
    }
}
