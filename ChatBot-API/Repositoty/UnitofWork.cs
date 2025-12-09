using ChatBot_API.Models;

namespace ChatBot_API.Repositoty
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IChatMessageRepository ChatMessages { get; private set; }
        public IMessageEditRepository MessageEdits { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ChatMessages = new ChatMessageRepository(context);
            MessageEdits = new MessageEditRepository(context);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }

}
