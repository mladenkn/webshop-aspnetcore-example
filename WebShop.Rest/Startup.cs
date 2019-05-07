using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using WebShop.DataAccess;
using WebShop.Services;

namespace WebShop.Rest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Webshop API", Version = "v1" });
            });

            services.AddMediatR(typeof(Logger).Assembly);

            services.AddEntityFrameworkInMemoryDatabase();
            services.AddDbContext<WebShopDbContext>(o => o.UseInMemoryDatabase("Webshop"));

            services.AddTransient<IBasketWithPriceCache, BasketWithPriceCache>();
            services.AddTransient<IQueries, Queries>();
            services.AddTransient<ISmartQueries, SmartQueries>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ISafeRunner, SafeRunner>();
            services.AddTransient<ICurrentUserProvider, CurrentUserProvider>();

            services.AddTransient<IBasketService, BasketService>();
            services.AddTransient<IRequestExecutor, RequestExecutor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Webshop API V1");
            });
        }
    }
}
