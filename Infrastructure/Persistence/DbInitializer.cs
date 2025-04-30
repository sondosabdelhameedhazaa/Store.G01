using Domain.Contracts;
using Domain.Models;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Persistence.Identity;

namespace Persistence
{
    public class DbIntializer : IDbInitializer
    {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _identityDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbIntializer(StoreDbContext context,
            StoreIdentityDbContext identityDbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole>roleManager
            )
        {
            _context = context;
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task IntializeAsync()
        {
            // Create Database if it doesnt exist && Apply to any pending Migrations
            if (_context.Database.GetPendingMigrations().Any())
            {
                await _context.Database.MigrateAsync();
            }
            // Data Seeding

            // 1) Seeding ProductTypes from json

            if (!_context.ProductTypes.Any())
            {
                // 1.Read Al Data from types json as string
                var TypesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");
                // 2.Transform string to C# Object "List<Product Types>"
                var types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                // 3.Add List to DB
                if (types is not null && types.Any())
                {
                    await _context.ProductTypes.AddRangeAsync(types);
                    await _context.SaveChangesAsync();
                }
            }
            // ..\Infrastructure\Persistence\Data\Seeding\types.json

            // 2) Seeding ProductBrands from json

            if (!_context.ProductBrands.Any())
            {
                // 1.Read Al Data from types json as string
                var BrandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");
                // 2.Transform string to C# Object "List<Product Types>"
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                // 3.Add List to DB
                if (brands is not null && brands.Any())
                {
                    await _context.ProductBrands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }
            }

            // 3) Seeding Products from json

            if (!_context.Products.Any())
            {
                // 1.Read Al Data from products json as string
                var ProductsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");
                // 2.Transform string to C# Object "List<Product Types>"
                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                // 3.Add List to DB
                if (products is not null && products.Any())
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task IntializeIdentityAsync()
        {
            if (_identityDbContext.Database.GetPendingMigrations().Any())
            {
                await _identityDbContext.Database.MigrateAsync();
            }

            //seeding
            //role

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Admin"
                });
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin"
                });
            }


            //users
            if (!_userManager.Users.Any())
            {
                var superAdmin = new AppUser()
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789"
                };
                var Admin = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "0123456789"
                };

                await _userManager.CreateAsync(superAdmin, "P@ssW0rd");
                await _userManager.CreateAsync(Admin, "P@ssW0rd");

                await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                await _userManager.AddToRoleAsync(Admin, "Admin");
            }

        }
        }
    }