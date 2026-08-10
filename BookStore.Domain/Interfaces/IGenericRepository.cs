using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BookStore.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : MainBaseEntity 
    {
        // Get all entities
        IQueryable<T> Query(bool includeDeleted = false);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asNoTracking = false
        );
        // Get all including deleted
        Task<IEnumerable<T>> GetAllWithDeletedAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asNoTracking = false);
        // Get single entity by condition
        Task<T?> GetAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asNoTracking = false
        );
        // Create
        System.Threading.Tasks.Task CreateAsync(T entity);
        // Update
        void Update(T entity);
        // Soft delete
        System.Threading.Tasks.Task SoftDeleteAsync(int id, string? deletedBy = null);
        // Hard delete (admin osnly)
        System.Threading.Tasks.Task HardDeleteAsync(int id);
        // Restore a soft deleted entity
        System.Threading.Tasks.Task RestoreAsync(int id);
    }
}
