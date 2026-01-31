using System.Threading.Tasks;
using Admin.Dashboard.Models.Products;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Service.Specification;
using ECommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Dashboard.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();

            var queryParams = new ProductQueryParams();
            var specification = new ProductWithBrandAndTypeSpecification(queryParams, true);

            var products = await productRepo.GetAllAsync(specification);

            var productsViewModel = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                BrandId = product.BrandId,
                TypeId = product.TypeId,
                Brand = product.ProductBrand,
                Type = product.ProductType
            });

            return View(productsViewModel);
        }
    }
}
