using Microsoft.Extensions.DependencyInjection;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.UnitsOfWork;
using DAL.Entities;

namespace DAL.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBaseRepository<Customer>>(provider => provider.GetService<ICustomerRepository>());

            services.AddScoped<IWorkerRepository, WorkerRepository>();
            services.AddScoped<IBaseRepository<Worker>>(provider => provider.GetService<IWorkerRepository>());

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBaseRepository<Product>>(provider => provider.GetService<IProductRepository>());

            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IBaseRepository<Position>>(provider => provider.GetService<IPositionRepository>());

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IBaseRepository<Department>>(provider => provider.GetService<IDepartmentRepository>());
            
            services.AddScoped<IDepartmentWorkerRepository, DepartmentWorkerRepository>();
            services.AddScoped<IBaseRepository<DepartmentWorker>>(provider => provider.GetService<IDepartmentWorkerRepository>());
            
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IBaseRepository<Order>>(provider => provider.GetService<IOrderRepository>());
            
            services.AddScoped<IOrderProductRepository, OrderProductRepository>();
            services.AddScoped<IBaseRepository<OrderProduct>>(provider => provider.GetService<IOrderProductRepository>());
            
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<IBaseRepository<Status>>(provider => provider.GetService<IStatusRepository>());
            
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IBaseRepository<Transaction>>(provider => provider.GetService<ITransactionRepository>());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
