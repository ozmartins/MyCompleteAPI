using Elmah.Io.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace API.Configuration
{
    public static class LogConfiguration
    {
        public static IServiceCollection AddLogConfiguration(this IServiceCollection services)
        {
            services.AddElmahIo(o => 
            { 
                o.ApiKey = "91426ea4a7764e74bb772d16d87fd510"; o.LogId = new Guid("2ac96656-ce9c-426e-9b03-b1d1b43ee491"); 
            });

            //Just if you want to use ILogger for manual logging.
            services.AddLogging(builder =>
            {
                builder.AddElmahIo(o =>
                {
                    o.ApiKey = "91426ea4a7764e74bb772d16d87fd510"; o.LogId = new Guid("2ac96656-ce9c-426e-9b03-b1d1b43ee491");
                });

                builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            });

            return services;
        }
    }
}
