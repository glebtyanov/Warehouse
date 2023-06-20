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

            services.AddAutoMapper(typeof(ModelToDtoProfile));

            return services;
        }
    }
}
