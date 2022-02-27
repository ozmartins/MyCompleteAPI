using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configuration
{
    public static class WebConfiguration
    {
        public static IServiceCollection AddWebConfiguration(this IServiceCollection services)
        {
            services.AddCors();
            
            services.AddControllers();
            
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(o => 
            { 
                o.GroupNameFormat = "'v'VVV"; 
                o.SubstituteApiVersionInUrl = true; 
            });

            services.Configure<ApiBehaviorOptions>(o => { o.SuppressModelStateInvalidFilter = true; });

            return services;
        }
    }
}
