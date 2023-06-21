using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext()
        {
                
        }
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {
        }

        virtual public DbSet<Customer> Customers { get; set; }
        virtual public DbSet<Department> Departments { get; set; }
        virtual public DbSet<DepartmentWorker> DepartmentWorkers { get; set; }
        virtual public DbSet<Order> Orders { get; set; }
        virtual public DbSet<OrderProduct> OrderProducts { get; set; }
        virtual public DbSet<Status> Statuses { get; set; }
        virtual public DbSet<Product> Products { get; set; }
        virtual public DbSet<Transaction> Transactions { get; set; }
        virtual public DbSet<Worker> Workers { get; set; }
        virtual public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WarehouseContext).Assembly);
        }
    }
}
