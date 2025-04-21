using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IProductService
    {
        // Get All Products
        Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters SpecParam);
        // Get product by Id
        Task<ProductResultDto?> GetProductByIdAsync(int id);
        // Get All Types
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();

        // Get All Brands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

    }
}