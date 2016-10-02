using HotDeliveryDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulerTasks
{
    public class Scheduler
    {
        public AppSettings AppSettings { get; set; }
        private IRepository Db { get; set; }
        public Scheduler()
        {
            AppSettings = _GetAppSetting();
            if (AppSettings.ConnectionStrings.DBFormat == "XMLFile")
                Db = new XmlRepository(AppSettings.ConnectionStrings.Path);
            else if (AppSettings.ConnectionStrings.DBFormat == "SQLite")
            {
                Db = new SQLiteRepository(AppSettings.ConnectionStrings.Path);

            }
        }

        public Scheduler(IRepository repository, AppSettings settings)
        {
            Db = repository;
            AppSettings = settings;
        }

        public Task CreateDeliveries(CancellationToken cancellationToken)
        {
            Random random = new Random();

            return Task.Run(() =>
            {
                while (true)
                {
                    int taskInterval = random.Next(AppSettings.TaskIntervalMin, AppSettings.TaskIntervalMax);
                    _CreatePoolDeliveriesAsync();
                    Thread.Sleep(taskInterval * 1000);
                    if (cancellationToken.IsCancellationRequested)
                        break;
                }

            }, cancellationToken);
        }

        public Task ExpireDeliveries(CancellationToken cancellationToken)
        {
            return Task.Run(() => _ExpireDeliveries(cancellationToken), cancellationToken);
        }

        private void _ExpireDeliveries(CancellationToken cancellationToken)
        {
            List<Delivery> deliveries = Db.GetDeliveryList();
            var subset =
                deliveries.Where(i => i.Status == "Available" && i.ExpirationTime < DateTime.Now);
            foreach (Delivery delivery in subset)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                delivery.Status = "Expired";
                Db.Update(delivery);
            }
        }

        private async void _CreatePoolDeliveriesAsync()
        {
            Random random = new Random();
            int deliveriesCount = random.Next(AppSettings.DeliveriesCountMin, AppSettings.DeliveriesCountMax);
            await Task.Run(() => _InsertRecords(deliveriesCount));
        }

        private void _InsertRecords(int k)
        {
            for (int i = 0; i < k; i++)
            {
                Delivery item = _CreateNewDelivery();
                Delivery itemDB = Db.Create(item);
                itemDB.ExpirationTime = itemDB.CreationTime.AddSeconds(AppSettings.ExpirationTime);
                Db.Update(itemDB);
            }
        }

        private Delivery _CreateNewDelivery()
        {
            Delivery delivery = new Delivery();
            delivery.Status = "Available";
            Random random = new Random();
            int title = random.Next();
            delivery.Title = title.ToString();
            delivery.UserId = null;
            return delivery;
        }

        private AppSettings _GetAppSetting()
        {
            AppSettings settings = new AppSettings();
            try
            {
                settings.ConnectionStrings.DBFormat = ConfigurationManager.AppSettings["DBFormat"];
                settings.ConnectionStrings.Path = ConfigurationManager.ConnectionStrings[settings.ConnectionStrings.DBFormat].ConnectionString;

                settings.DeliveriesCountMin = Int32.Parse(ConfigurationManager.AppSettings["DeliveriesCountMin"]);
                settings.DeliveriesCountMax = Int32.Parse(ConfigurationManager.AppSettings["DeliveriesCountMax"]);
                settings.TaskIntervalMin = Int32.Parse(ConfigurationManager.AppSettings["TaskIntervalMin"]);
                settings.TaskIntervalMax = Int32.Parse(ConfigurationManager.AppSettings["TaskIntervalMax"]);
                settings.ExpirationTime = Int32.Parse(ConfigurationManager.AppSettings["ExpirationTime"]);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            //int expirationTime;
            //if (Int32.TryParse(ConfigurationManager.AppSettings["ExpirationTime"], out expirationTime))

            //    settings.ExpirationTime = expirationTime;
            //else
            //    settings.IsValid = false;

            return settings;
        }

    }
}
