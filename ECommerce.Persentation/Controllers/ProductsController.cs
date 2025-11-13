using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Service.Abstraction;
using ECommerce.Shared;
using ECommerce.Shared.DTOS.ProductDtos;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Persentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #region Get All Products

        [HttpGet]
        // BaseUrl/api/Products
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var Products = await _productService.GetAllProductsAsync(queryParams);
            return Ok(Products);
        }

        #endregion

        #region Get Product By Id

        [HttpGet("{id}")]
        // BaseUrl/api/Products/3
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var Product = await _productService.GetProductByIdAsync(id);
            return Ok(Product);
        }

        #endregion

        #region Get All Brands

        [HttpGet("brands")]
        // BaseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var Brands = await _productService.GetAllBrandsAsync();
            return Ok(Brands);
        }

        #endregion

        #region Get All Types

        [HttpGet("types")]
        // BaseUrl/api/Products/types
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var Types = await _productService.GetAllTypesAsync();
            return Ok(Types);
        }

        #endregion
    }
}
