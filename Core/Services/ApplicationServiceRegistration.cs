﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using ServicesAbstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(AssemblyReference).Assembly);
            services.AddScoped<IServiceManager, ServiceManager>();
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            return services;
        }

    }
}