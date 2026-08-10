using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Presistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Task_Manager.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : MainBaseEntity
    {
        protected readonly BookStoreDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(BookStoreDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Query(bool includeDeleted = false)
        {
            return includeDeleted ? _dbSet.IgnoreQueryFilters() : _dbSet;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool asNoTracking = false)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null) query = query.Where(predicate);
            if (include != null) query = include(query);
            if (asNoTracking) query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithDeletedAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool asNoTracking = false)
        {
            IQueryable<T> query = _dbSet.IgnoreQueryFilters();
            if (predicate != null) query = query.Where(predicate);
            if (include != null) query = include(query);
            if (asNoTracking) query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool asNoTracking = false)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (include != null) query = include(query);
            if (asNoTracking) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = null;
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }

        public async Task SoftDeleteAsync(int id, string? deletedBy = null)
        {
            var entity = await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) return;
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }

        public async Task HardDeleteAsync(int id)
        {
            var entity = await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) return;
            _dbSet.Remove(entity);
        }

        public async Task RestoreAsync(int id)
        {
            var entity = await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) return;
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            _dbSet.Update(entity);
        }
    }
}
