using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using ServicesAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IBasketRepository basketRepository,
        ICasheRepository casheRepository,
        UserManager<AppUser> userManager
        
        ) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork, mapper);
        public IBasketService basketService { get; } = new BasketService(basketRepository, mapper);

        public ICasheService casheService { get; } = new CasheService(casheRepository);

        public IAuthService AuthService { get; } = new AuthService(userManager);
    }
}