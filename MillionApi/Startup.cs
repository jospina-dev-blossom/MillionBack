using MillionApi.Application.Interfaces;
using MillionApi.Infrastructure.Data;
using MillionApi.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MillionApi.Application.Queries;
using MillionApi.Api.Middleware;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace MillionApi.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MongoSettings>(Configuration.GetSection("MongoSettings"));
            services.AddSingleton<MongoContext>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPropertiesQueryHandler).Assembly));
            services.AddAutoMapper(cfg => { }, typeof(GetPropertiesQueryHandler).Assembly);

            services.AddControllers();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(typeof(GetPropertiesQueryHandler).Assembly);

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Million Properties API",
                Version = "v1",
                Description = "API para gestión de propiedades con manejo de excepciones personalizado",
                Contact = new OpenApiContact
                {
                    Name = "Million Properties Team"
                }
            });

            // Habilitar comentarios XML
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Agregar el middleware de manejo de excepciones PRIMERO
            app.UseExceptionHandling();

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
