using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using WooliesX.Services.Interfaces;
using WooliesXTest.Infrastructure.SortStrategies;
using WooliesXTest.Infrastructure.SortStrategies.Interfaces;
using WooliesXTest.Options;
using WooliesXTest.Provider;
using WooliesXTest.Services;
using WooliesXTest.Utility;

namespace WooliesXTest
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
            services.AddControllers();
            services.AddHttpClient();
            services.AddOptions();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WooliesXTest-Api", Version = "v1" });
            });

            services.Configure<WooliexApiOptions>(Configuration.GetSection("WooliexApiOptions"));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductDataProvider, ProductDataProvider>();
            services.AddTransient<ProductsPriceAscending>();
            services.AddTransient<ProductsPriceDecending>();
            services.AddTransient<ProductsNameAscending>();
            services.AddTransient<ProductsNameDecending>();
            services.AddTransient<RecommendedProducts>();

            services.AddTransient<Func<string, IProductsSortStrategy>>(serviceProvider => key =>
            {
                switch (key.ToLower())
                {
                    case Constants.Low:
                        return serviceProvider.GetService<ProductsPriceAscending>();
                    case Constants.High:
                        return serviceProvider.GetService<ProductsPriceDecending>();
                    case Constants.Ascending:
                        return serviceProvider.GetService<ProductsNameAscending>();
                    case Constants.Descending:
                        return serviceProvider.GetService<ProductsNameDecending>();
                    case Constants.Recommended:
                        return serviceProvider.GetService<RecommendedProducts>();
                    default:
                        throw new Exception("Invalid Sort Option");
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("v1/swagger.json", "WooliesXTest-Api-V1");
            });
        }
    }
}
