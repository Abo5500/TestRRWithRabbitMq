using Application.Interfaces.Email;
using Application.Interfaces.PriceItems;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //TODO: Здесь должен быть кастомный exception
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("DefaultConnection not found");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IEmailContainsFileChecker, EmailImapChecker>();
            services.AddScoped<IEmailFileLoader, EmailImapLoader>();
            services.AddScoped<IPriceItemRepository, PriceItemRepository>();

            services.Configure<EmailOptions>(configuration.GetSection("EmailConfigurations"));


            return services;
        }
    }
}
