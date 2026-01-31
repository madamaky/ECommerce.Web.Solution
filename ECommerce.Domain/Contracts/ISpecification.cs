using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Contracts
{
    public interface ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        // Include
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; }

        // Where
        public Expression<Func<TEntity, bool>> Criteria { get; }

        // Sorting
        public Expression<Func<TEntity, object>> OrderBy { get; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; }

        // Pagination
        public int Skip { get; }
        public int Take { get; }
        public bool IsPaginated { get; }
    }
}
