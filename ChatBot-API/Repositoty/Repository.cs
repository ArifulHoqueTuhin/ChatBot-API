using System.Linq.Expressions;
using ChatBot_API.Data;
using ChatBot_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ChatBot_API.Repositoty
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbset = _context.Set<T>();

        }
        public IQueryable<T> Entities => dbset;
        public IQueryable<T> ActiveEntities => dbset.Where(x => x.IsSoftDeleted == true);
        public async Task<List<T>> GetAllAsync() => await dbset.ToListAsync();
        public async Task<T> GetByIdAsync(int id)
        {
            var data = await dbset.FindAsync(id);
            if (data == null)
            {
                throw new KeyNotFoundException($"{typeof(T).Name} with Id {id} not found.");
            }
            return data;
        }
        public async Task AddAsync(T entity) => await dbset.AddAsync(entity);
        public void Update(T entity) => dbset.Update(entity);
        public void Delete(T entity) => dbset.Remove(entity);
        public async Task<List<T>> GetPaginatedMessagesAsync(int page, int pageSize)
         => await dbset
             .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .AsNoTracking()
             .ToListAsync();

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> filter, int page, int pageSize)
        {
            return await dbset.Where(filter).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

    }
}
