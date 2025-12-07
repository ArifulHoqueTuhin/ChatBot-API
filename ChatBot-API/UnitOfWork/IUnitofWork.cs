using ChatBot_API.Models;
using ChatBot_API.Repositoty;

namespace ChatBot_API.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> SaveAsync();
    }
}

