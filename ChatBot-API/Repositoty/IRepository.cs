

using System.Linq.Expressions;
using ChatBot_API.Models;

namespace ChatBot_API.Repositoty
{
   public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Entities { get; }
        IQueryable<T> ActiveEntities { get; }
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<List<T>> GetPaginatedMessagesAsync(int page, int pageSize);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter, int page, int pageSize); // by userid, sessionid, messageid

    }
}

