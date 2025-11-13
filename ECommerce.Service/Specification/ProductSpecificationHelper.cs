using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;

namespace ECommerce.Service.Specification
{
    public class ProductSpecificationHelper
    {
        //: base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value) &&
        //      (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value) &&
        //      (string.IsNullOrEmpty(queryParams.Search) || P.Name.ToLower().Contains(queryParams.Search.ToLower())))

        public static Expression<Func<Product, bool>> GetProductCriteria(ProductQueryParams queryParams)
        {
            return P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value) &&
                        (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value) &&
                        (string.IsNullOrEmpty(queryParams.Search) || P.Name.ToLower().Contains(queryParams.Search.ToLower()));
        }
    }
}
