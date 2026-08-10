using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Presistance.Context;

namespace Task_Manager.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BookStoreDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        public UnitOfWork(BookStoreDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<T> Repository<T>() where T : MainBaseEntity
        {
            var type = typeof(T);

            if (!_repositories.TryGetValue(type, out var repository))
            {
                var repoInstance = new GenericRepository<T>(_context);
                _repositories[type] = repoInstance;
                return repoInstance;
            }

            return (IGenericRepository<T>)repository!;
        }
        public async Task<bool> SaveAsync()
        {
            bool result = await _context.SaveChangesAsync() != 0;
            return result;
        }
        public void Dispose() => _context.Dispose();
    }
}
