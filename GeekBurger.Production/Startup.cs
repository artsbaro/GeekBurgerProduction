using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using GeekBurger.Production.Repository.Interfaces;
using GeekBurger.Production.Repository;
using GeekBurger.Production.Service.Interfaces;
using GeekBurger.Production.Service;
using GeekBurger.Production.Configuration;
using Microsoft.Extensions.Options;

namespace GeekBurger.Production
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appSettings.json").Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                            new OpenApiInfo
                            {
                                Title = "Production",
                                Version = "v1",
                                Description = "GeekBurger Production Microservice Web API",
                            });
            });

            services.AddAutoMapper();

            services.AddSingleton<IReceiveMessagesFactory, ReceiveMessagesFactory>();
            
            services.AddSingleton<IProductionRepository, ProductionRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();

            services.AddSingleton<IProductionService, ProductionService>();
            services.AddSingleton<IOrderService, OrderService>();            
            services.AddSingleton<IProductionAreaChangedService, ProductionAreaChangedService>();
            services.AddSingleton<IServiceBusService, ServiceBusService>();            

            services.Configure<ProductionDatabaseSettings>(
                Configuration.GetSection(nameof(ProductionDatabaseSettings)));

            services.AddSingleton<IProductionDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ProductionDatabaseSettings>>().Value);

            services.Configure<NewOrderDatabaseSettings>(
                Configuration.GetSection(nameof(NewOrderDatabaseSettings)));

            services.AddSingleton<INewOrderDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<NewOrderDatabaseSettings>>().Value);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("https://geekburgerproductionacdw.azurewebsites.net/swagger")
                    .AllowAnyHeader()
                    );
            });
            var mvcCoreBuilder = services.AddMvcCore();

            mvcCoreBuilder
                .AddFormatterMappings()
                .AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseCors("AllowAll");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Production");
            });

            app.ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetService<IReceiveMessagesFactory>();

            app.ApplicationServices.CreateScope()
                .ServiceProvider.
                GetService<IServiceBusService>()
                .CriarTopico();

            app.Run(async (context) =>
            {
                await context.Response
                   .WriteAsync("GeekBurger.Production running");
            });
        }
    }
}
