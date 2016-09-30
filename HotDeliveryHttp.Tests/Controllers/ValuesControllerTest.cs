using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotDeliveryHttp;
using HotDeliveryHttp.Controllers;
using HotDeliveryDB;

namespace HotDeliveryHttp.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void GetAvailableDeliveries()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            IEnumerable<Delivery> result = controller.GetAvailableDeliveries();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.AreEqual("423247148", result.ElementAt(0).Title);
         //   Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void TakeDelivery()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            controller.TakeDelivery(2 , 55);

            // Assert
        }

      
    }
}
