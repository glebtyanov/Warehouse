using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentWorker> DepartmentWorkers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Worker> Workers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WarehouseContext).Assembly);
        }
    }
}
