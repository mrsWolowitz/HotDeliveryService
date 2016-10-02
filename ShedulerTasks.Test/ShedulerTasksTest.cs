using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotDeliveryDB;
using Moq;
using System.Collections.Generic;
using System.Linq;
using SchedulerTasks;
using System.Threading;
using System.Threading.Tasks;
using HotDeliveryDB.Types;

namespace ShedulerTasks.Test
{
    [TestClass]
    public class ShedulerTasksTest
    {
        static AppSettings _Settings { get; set; }
        static IRepository _MockRepository { get; set; }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _Settings = _CreateSettings();
            _MockRepository = _CreateMockRepository();
        }

        #region ClassInitialize
        private static IRepository _CreateMockRepository()
        {
            DateTime dateTime = DateTime.Now.AddHours(-1);
            var deliveries = new List<Delivery> {
        new Delivery {
            Id =1,
            CreationTime = dateTime,
            Status = Enum.GetName(typeof(Status), Status.Expired),
            Title = "1111111",
            ModificationTime = dateTime.AddSeconds(_Settings.ExpirationTime + 30),
            ExpirationTime = dateTime.AddSeconds(_Settings.ExpirationTime) },
        new Delivery {
            Id =2,
            CreationTime = dateTime,
            Status = Enum.GetName(typeof(Status), Status.Available),
            Title = "2222222",
            ModificationTime = dateTime,
            ExpirationTime = dateTime.AddSeconds(_Settings.ExpirationTime)},
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

            mock.Setup(m => m.Update(It.IsAny<Delivery>()))
                .Callback<Delivery>(u =>
                {
                    var original = deliveries.Where(q => q.Id == u.Id).Single();
                    original.Id = u.Id;
                }).Verifiable();

            return mock.Object;
        }

        private static AppSettings _CreateSettings()
        {
            AppSettings settings = new AppSettings();
            settings.TaskIntervalMin = 10;
            settings.TaskIntervalMax = 20;
            settings.DeliveriesCountMin = 3;
            settings.DeliveriesCountMax = 6;
            settings.ExpirationTime = 5;

            return settings;
        } 
        #endregion

        [TestMethod]
        public void CreateDeliveries_DeliveriesCountIncrease()
        {
            //Arrange      
            int countBeforeCreating = _MockRepository.GetDeliveryList().Count();
            int expectedMinCount = countBeforeCreating + _Settings.DeliveriesCountMin;
            int expectedMaxCount = countBeforeCreating + _Settings.DeliveriesCountMax;

            Scheduler scheduler = new Scheduler(_MockRepository, _Settings);

            //Act
            using (CancellationTokenSource _Cts = new CancellationTokenSource())
            {
                scheduler.CreateDeliveries(_Cts.Token);
                Thread.Sleep(900);
                _Cts.Cancel();
            }
            int countAfterCreating = _MockRepository.GetDeliveryList().Count();

            //Assert
            Assert.IsTrue(countBeforeCreating < countAfterCreating);
            Assert.IsTrue(countAfterCreating >= expectedMinCount);
            Assert.IsTrue(countAfterCreating <= expectedMaxCount);
        }

        [TestMethod]
        public void ExpireDeliveries_SetStatusExpired()
        {
            //Arrange
            string expectedStatus = Enum.GetName(typeof(Status), Status.Expired);
            Scheduler scheduler = new Scheduler(_MockRepository, _Settings);

            //Act
            using (CancellationTokenSource _Cts = new CancellationTokenSource())
            {
                scheduler.ExpireDeliveries(_Cts.Token);
                Thread.Sleep(900);
                _Cts.Cancel();
            }

            //Assert
            Assert.AreEqual(expectedStatus, _MockRepository.GetDelivery(2).Status);
        }
    }
}
