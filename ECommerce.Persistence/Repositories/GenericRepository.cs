using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
            => await _dbContext.Set<TEntity>().AddAsync(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id)
            => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Remove(TEntity entity)
            => _dbContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity)
            => _dbContext.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification)
        {
            var Query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification);
            return await Query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification)
            => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).FirstOrDefaultAsync();

        public async Task<int> CountAsync(ISpecification<TEntity, TKey> specification)
            => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).CountAsync();
    }
}
