using Hard.Business.Interfaces;
using Hard.Data.Context;
using Hard.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)        
        {
            services.AddScoped<HardDbContext>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();


            return services;
        }
    }
}
