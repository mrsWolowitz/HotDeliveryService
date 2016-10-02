using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotDeliveryHttp;
using HotDeliveryHttp.Controllers;
using HotDeliveryDB;
using System.Web.Http.Results;
using Moq;
using HotDeliveryDB.Types;

namespace HotDeliveryHttp.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        private IRepository _CreateMockRepository()
        {
            DateTime dateTime = DateTime.Now.AddHours(-1);
            var deliveries = new List<Delivery> {
        new Delivery {
            Id =1,
            CreationTime = dateTime,
            Status = Enum.GetName(typeof(Status), Status.Expired),
            Title = "1111111",
            ModificationTime = dateTime.AddMinutes(1),
            ExpirationTime = dateTime.AddSeconds(5) },
        new Delivery {
            Id =2,
            CreationTime = dateTime,
            Status = Enum.GetName(typeof(Status), Status.Available),
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
            ValuesController controller = new ValuesController(mockRepository);

            // Act
            IHttpActionResult actionResult = controller.GetAvailableDeliveries();

            // Assert
            var result = actionResult as OkNegotiatedContentResult<IEnumerable<Delivery>>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.Count());
        }

        [TestMethod]
        public void GetAvailableDeliveries_InternalServerError()
        {
            // Arrange
            Mock<IRepository> mock = new Mock<IRepository>();
            ValuesController controller = new ValuesController(mock.Object);

            // Act
            IHttpActionResult actionResult = controller.GetAvailableDeliveries();

            // Assert
            var result = actionResult as NegotiatedContentResult<string>;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("InternalServerError", result.Content);
        }

        [TestMethod]
        public void TakeDelivery_Ok()
        {
            // Arrange
            var mockRepository = _CreateMockRepository();
            ValuesController controller = new ValuesController(mockRepository);

            // Act
            IHttpActionResult actionResult = controller.TakeDelivery(2 , 55);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));            
        }

        [TestMethod]
        public void TakeDelivery_SetStatusTaken()
        {
            // Arrange
            var mockRepository = _CreateMockRepository();
            ValuesController controller = new ValuesController(mockRepository);
            string expectedStatus = Enum.GetName(typeof(Status), Status.Taken);
            int id = 2;

            // Act
            IHttpActionResult actionResult = controller.TakeDelivery(id, 55);

            // Assert
            Assert.AreEqual(expectedStatus, mockRepository.GetDelivery(2).Status);
        }

        [TestMethod]
        public void TakeDelivery_NotFound()
        {
            // Arrange
            var mockRepository = _CreateMockRepository();
            ValuesController controller = new ValuesController(mockRepository);

            // Act
            IHttpActionResult actionResult = controller.TakeDelivery(7, 55);

            // Assert
            var result = actionResult as NegotiatedContentResult<string>;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Доставка не найдена", result.Content);
        }

        [TestMethod]
        public void TakeDelivery_NotAvailable()
        {
            // Arrange
            var mockRepository = _CreateMockRepository();
            ValuesController controller = new ValuesController(mockRepository);

            // Act
            IHttpActionResult actionResult = controller.TakeDelivery(1, 55);

            // Assert
            var result = actionResult as NegotiatedContentResult<string>;
            Assert.IsNotNull(result);
            Assert.AreEqual((HttpStatusCode)422, result.StatusCode);
            Assert.AreEqual("Статус доставки не Available", result.Content);
        }

        //[TestMethod]
        //public void TakeDelivery_InternalServerError()
        //{
        //    // Arrange
        //    Mock<IRepository> mock = new Mock<IRepository>();
        //    ValuesController controller = new ValuesController(mock.Object);

        //    // Act
        //    IHttpActionResult actionResult = controller.TakeDelivery(-2, 55);

        //    // Assert
        //    var result = actionResult as NegotiatedContentResult<string>;
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
        //    Assert.AreEqual("InternalServerError", result.Content);
        //}
    }
}
