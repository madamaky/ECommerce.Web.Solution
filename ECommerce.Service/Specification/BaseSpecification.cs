using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;

namespace ECommerce.Service.Specification
{
    public class BaseSpecification<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        #region Includes
        
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];

        // Method To Add Includes To Property
        protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
            => IncludeExpression.Add(includeExp);

        #endregion

        #region Where

        public Expression<Func<TEntity, bool>> Criteria { get; }

        public BaseSpecification(Expression<Func<TEntity, bool>> CriteriaExp)
            => Criteria = CriteriaExp;

        #endregion

        #region Sorting

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExp)
            => OrderBy = orderByExp;
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExp)
            => OrderByDescending = orderByDescendingExp;

        #endregion

        #region Pagination

        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginated { get; set; }

        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = pageSize * (pageIndex - 1);
        }

        #endregion
    }
}
