﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Abstractions;

namespace ServicesAbstractions
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        IBasketService basketService { get; }
        ICasheService casheService { get; }

         

    }
}