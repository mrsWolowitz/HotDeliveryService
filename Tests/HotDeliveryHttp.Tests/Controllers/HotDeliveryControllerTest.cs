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

using System.Web.Http.Results;
using Moq;
using HotDeliveryModel;

namespace HotDeliveryHttp.Tests.Controllers
{
    [TestClass]
    public class HotDeliveryControllerTest
    {
        [TestMethod]
        public void GetAvailableDeliveries_Ok()
        {
            //Arrange
            Mock<IHotDeliveryViewModel> mock = new Mock<IHotDeliveryViewModel>();
            var deliveries = new List<DeliveryDTO> { new DeliveryDTO() };
            mock.Setup(m => m.GetAvailableDeliveries()).Returns(deliveries);
            var viewModel = mock.Object;

            var controller = new HotDeliveryController(viewModel);
            int expectedCount = 1;

            // Act
            IHttpActionResult actionResult = controller.GetAvailableDeliveries();

            // Assert
            var result = actionResult as OkNegotiatedContentResult<List<DeliveryDTO>>;

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Content.Count());
        }

        [TestMethod]
        public void GetAvailableDeliveries_Exception()
        {
            //Arrange
            Mock<IHotDeliveryViewModel> mock = new Mock<IHotDeliveryViewModel>();
            mock.Setup(m => m.GetAvailableDeliveries()).Throws<Exception>();
            var viewModel = mock.Object;

            var controller = new HotDeliveryController(viewModel);

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
            Mock<IHotDeliveryViewModel> mock = new Mock<IHotDeliveryViewModel>();
            mock.Setup(m => m.TakeDelivery(2, 55)).Returns(new ResponseDTO(ResponseType.Ok));
            var viewModel = mock.Object;

            var controller = new HotDeliveryController(viewModel);

            // Act
            IHttpActionResult actionResult = controller.TakeDelivery(2, 55);

            // Assert
            var result = actionResult as NegotiatedContentResult<string>;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void TakeDelivery_NotFound()
        {
            // Arrange
            Mock<IHotDeliveryViewModel> mock = new Mock<IHotDeliveryViewModel>();
            mock.Setup(m => m.TakeDelivery(2, 55)).Returns(new ResponseDTO(ResponseType.NotFound, "Доставка не найдена"));
            var viewModel = mock.Object;

            var controller = new HotDeliveryController(viewModel);

            // Act
            IHttpActionResult actionResult = controller.TakeDelivery(2, 55);

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
            Mock<IHotDeliveryViewModel> mock = new Mock<IHotDeliveryViewModel>();
            mock.Setup(m => m.TakeDelivery(2, 55)).Returns(new ResponseDTO(ResponseType.NotAvailable, "Статус доставки не Available"));
            var viewModel = mock.Object;

            var controller = new HotDeliveryController(viewModel);

            // Act
            IHttpActionResult actionResult = controller.TakeDelivery(2, 55);

            // Assert
            var result = actionResult as NegotiatedContentResult<string>;
            Assert.IsNotNull(result);
            Assert.AreEqual((HttpStatusCode)422, result.StatusCode);
            Assert.AreEqual("Статус доставки не Available", result.Content);
        }

        [TestMethod]
        public void TakeDelivery_Exception()
        {
            //Arrange
            Mock<IHotDeliveryViewModel> mock = new Mock<IHotDeliveryViewModel>();
            mock.Setup(m => m.TakeDelivery(2, 55)).Throws<Exception>();
            var viewModel = mock.Object;

            var controller = new HotDeliveryController(viewModel);

            // Act
            IHttpActionResult actionResult = controller.TakeDelivery(2, 55);

            // Assert
            var result = actionResult as NegotiatedContentResult<string>;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("InternalServerError", result.Content);
        }

    }
}
