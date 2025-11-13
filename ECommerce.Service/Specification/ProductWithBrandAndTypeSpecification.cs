using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;

namespace ECommerce.Service.Specification
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product, int>
    {
        public ProductWithBrandAndTypeSpecification(ProductQueryParams queryParams)
            : base(ProductSpecificationHelper.GetProductCriteria(queryParams))
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

            switch (queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(P => P.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(P => P.Price);
                    break;
                default:
                    AddOrderBy(P => P.Id);
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
    }
}
