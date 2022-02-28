using API.Configuration;
using API.Extensions;
using Hard.Data.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HardDbContext>(options => { options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); });
            services.AddWebConfiguration();
            services.AddAutoMapper(typeof(Startup));
            services.ResolveDependencies();
            services.AddIdentityConfiguration(Configuration);
            services.AddJwtConfiguration(Configuration);
            services.AddSwaggerConfig();
            services.AddLogConfiguration();
            services.AddHealthChecks()
                .AddElmahIoPublisher(o => { o.ApiKey = "91426ea4a7764e74bb772d16d87fd510"; o.LogId = new Guid("2ac96656-ce9c-426e-9b03-b1d1b43ee491"); })
                .AddCheck("Data", new DataHealthCheck(Configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(Configuration.GetConnectionString("DefaultConnection"), name: "DB");
            services.AddHealthChecksUI().AddSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                                
            }

            app.UseSwaggerConfig(provider);

            app.UseElmahIo();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();                      

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseHealthChecks("/hc", new HealthCheckOptions() { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

            app.UseHealthChecksUI(o => o.UIPath = "/hc-ui");
        }
    }
}
