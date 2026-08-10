using BookStore.Domain.Entities;

namespace BookStore.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : MainBaseEntity;
        Task<bool> SaveAsync();
    }
}
