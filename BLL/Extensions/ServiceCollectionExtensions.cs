using Microsoft.Extensions.DependencyInjection;
using BLL.MappingProfiles;
using BLL.Services;

namespace BLL.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddBusinessLogicLayerServices(this IServiceCollection services)
        {
            services.AddScoped<CustomerService, CustomerService>();
            services.AddScoped<DepartmentService, DepartmentService>();
            services.AddScoped<OrderService, OrderService>();
            services.AddScoped<ProductService, ProductService>();
            services.AddScoped<PositionService, PositionService>();
            services.AddScoped<StatusService, StatusService>();
            services.AddScoped<AuthService, AuthService>();
            services.AddScoped<TransactionService, TransactionService>();
            services.AddScoped<WorkerService, WorkerService>();

            services.AddAutoMapper(typeof(ModelToDtoProfile));

            return services;
        }
    }
}
