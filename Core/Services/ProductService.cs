﻿using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Services.Specifications;
using ServicesAbstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {


        public async Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters SpecParam)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(SpecParam);

            // Get all products through Product Repository
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);
            // Mapping IEnumerable Type -> DTO
            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);

            var specCount = new ProductWithCountSpecification(SpecParam);

            var count = await unitOfWork.GetRepository<Product, int>().CountAsync(specCount);

            return new PaginationResponse<ProductResultDto>(SpecParam.PageIndex, SpecParam.PageSize, count, result);

        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            if (product is null) throw new ProductNotFoundException(id);

            var result = mapper.Map<ProductResultDto>(product);
            return result;
        }

        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return result;
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }


    }
}