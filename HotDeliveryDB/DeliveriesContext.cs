using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using HotDeliveryDB.Types;

namespace HotDeliveryDB
{
    public class DeliveriesContext : DbContext
    {
        public DbSet<Delivery> Deliveries { get; set; }
        private string _ConnectionString { get; set; }

        public DeliveriesContext(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Delivery>().HasKey(m => m.Id);
            builder.Entity<Delivery>()
                .Property(b => b.Status)
                .ForSqliteHasDefaultValue(Status.Available);
            builder.Entity<Delivery>()
                .Property(b => b.CreationTime)
                .ForSqliteHasDefaultValueSql("DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME')");
            builder.Entity<Delivery>()
                .Property(b => b.ModificationTime)
                .ForSqliteHasDefaultValueSql("DATETIME(CURRENT_TIMESTAMP, 'LOCALTIME')");
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = _ConnectionString };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }
    }
}
