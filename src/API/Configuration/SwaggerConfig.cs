using API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace API.Configuration
{
    public static class SwaggerConfig 
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();
                
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });                
            });

            return services;            
        }

        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();

            app.UseSwaggerUI(o => 
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    o.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",description.GroupName.ToUpperInvariant());
                }
            });

            return app;
        }
    }

    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated = apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                parameter.Required |= description.IsRequired;
            }

        }
    }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private const string MIT_LICENCE = "https://opensource/org/licence/MIT";
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "API - Desenvolvedor.io",
                Version = description.ApiVersion.ToString(),
                Description = "Esta API faz parte do curso REST com ASP.NET Core WebAPI.",
                Contact = new OpenApiContact() { Name = "Oséias da Silva Martins", Email = "cadastro.osm@gmail.com" },
                TermsOfService = new Uri(MIT_LICENCE),
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri(MIT_LICENCE) }
            };

            if (description.IsDeprecated)
            {
                info.Description += " Esta versão está obsoleta";
            }

            return info;
        }
    }    
}
