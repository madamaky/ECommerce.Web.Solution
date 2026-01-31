using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence
{
    public class SpecificationEvaluator
    {
        // Method To Create Query
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> EntryPoint, ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var Query = EntryPoint;

            if (specification is not null)
            {
                #region Where

                if (specification.Criteria is not null)
                    Query = Query.Where(specification.Criteria);

                #endregion

                #region Includes

                if (specification.IncludeExpression is not null && specification.IncludeExpression.Any())
                {
                    //foreach (var IncludeExp in specification.IncludeExpression)
                    //    Query = Query.Include(IncludeExp);

                    Query = specification.IncludeExpression.Aggregate(Query, (CurrentQuery, IncludeExp) =>
                        CurrentQuery.Include(IncludeExp));
                }

                #endregion

                #region Sorting

                if (specification.OrderBy is not null)
                    Query = Query.OrderBy(specification.OrderBy);

                if (specification.OrderByDescending is not null)
                    Query = Query.OrderByDescending(specification.OrderByDescending);

                #endregion

                #region Pagination

                if (specification.IsPaginated)
                    Query = Query.Skip(specification.Skip).Take(specification.Take);

                #endregion
            }

            return Query;
        }
    }
}
