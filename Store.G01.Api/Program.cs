
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence;
using Persistence.Data;
using Services;
using Services.Abstractions;
using Shared.errorModels;
using Store.G01.Api.Extensions;
using Store.G01.Api.Middlewares;
using AssemblyMapping = Services.AssemblyReference;

namespace Store.G01.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          




            builder.Services.RegisterAllServices(builder.Configuration);

            var app = builder.Build();




            await app.ConfigureMiddleWares();

            app.Run();
        }
    }
}
