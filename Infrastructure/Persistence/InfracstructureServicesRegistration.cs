﻿using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Data.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class InfracstructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) // this for extension methods
        {

            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IDbInitializer, DbIntializer>(); // Allow DI 
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!);
            });


            return services;
        }
    }
}