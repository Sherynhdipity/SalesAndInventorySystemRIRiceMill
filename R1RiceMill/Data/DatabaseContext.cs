using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using R1RiceMill.Core;

namespace R1RiceMill.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Batch> Batches { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Return> Returns { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("config.json", false)
                .Build();
            optionsBuilder.UseSqlServer(config["connectionString"]);
            base.OnConfiguring(optionsBuilder);
        }

#if DEBUG
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    FirstName = "First",
                    MiddleName = "Middle",
                    LastName = "Last",
                    Username = "admin",
                    Password = "admin",
                    IsActive = true,
                    Role = Role.Administrator
                },
                new User()
                {
                    Id = 2,
                    FirstName = "First",
                    MiddleName = "Middle",
                    LastName = "Last",
                    Username = "cashier",
                    Password = "cashier",
                    IsActive = true,
                    Role = Role.Cashier
                },
                new User()
                {
                    Id = 3,
                    FirstName = "First",
                    MiddleName = "Middle",
                    LastName = "Last",
                    Username = "inventory",
                    Password = "inventory",
                    IsActive = true,
                    Role = Role.InventoryClerk
                }
            );
        }
#endif
    }
}
