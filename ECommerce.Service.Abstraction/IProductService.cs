using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared;
using ECommerce.Shared.DTOS.ProductDtos;

namespace ECommerce.Service.Abstraction
{
    public interface IProductService
    {
        //Data Transfer Object => DTO == View Model

        Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams);
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();
        Task<IEnumerable<TypeDTO>> GetAllTypesAsync();
    }
}
