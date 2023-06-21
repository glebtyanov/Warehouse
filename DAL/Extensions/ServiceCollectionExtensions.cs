using Microsoft.Extensions.DependencyInjection;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.UnitsOfWork;

namespace DAL.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IWorkerRepository, WorkerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentWorkerRepository, DepartmentWorkerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderProductRepository, OrderProductRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
