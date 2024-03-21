using Application.Interfaces.PriceItems;
using Application.Options;
using Application.PriceItems.BackgroundServices;
using Application.PriceItems.PriceItemsTaker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<List<SupplierOptions>>(configuration.GetSection("SuppliersSettings"));

            services.AddScoped<IPriceItemTaker, PriceItemTaker>();
            services.AddHostedService<PriceListBackgroundLoader>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
