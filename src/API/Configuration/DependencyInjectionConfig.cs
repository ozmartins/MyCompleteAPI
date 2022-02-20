using API.Extensions;
using Business.Interfaces;
using Hard.Business.Interfaces;
using Hard.Business.Notifications;
using Hard.Business.Services;
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

            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<INotifier, Notifier>();

            services.AddScoped<IUser, AspNetUser>();

            return services;
        }
    }
}
