
using ChatBot_API.Data;
using ChatBot_API.Models;
using ChatBot_API.Repositoty;


namespace ChatBot_API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<string, object> _repositories = new();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repoType = typeof(Repository<>).MakeGenericType(typeof(TEntity));
                var repoInstance = Activator.CreateInstance(repoType, _context)!;
                _repositories.Add(type, repoInstance);
            }

            return (IRepository<TEntity>)_repositories[type]!;
        }
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context?.Dispose();
        public ValueTask DisposeAsync() => _context.DisposeAsync();
    }


}
