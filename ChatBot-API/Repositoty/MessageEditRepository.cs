using ChatBot_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBot_API.Repositoty
{
    public class MessageEditRepository : IMessageEditRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageEditRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MessageEdit>> GetAllAsync() => await _context.MessageEdits.ToListAsync();

        public async Task<MessageEdit> GetByIdAsync(int id) => await _context.MessageEdits.FindAsync(id);

        public async Task AddAsync(MessageEdit entity) => await _context.MessageEdits.AddAsync(entity);

        public void Update(MessageEdit entity) => _context.MessageEdits.Update(entity);

        public void Delete(MessageEdit entity) => _context.MessageEdits.Remove(entity);

        public async Task<IEnumerable<MessageEdit>> GetEditsByMessageIdAsync(int messageId) =>
            
            await _context.MessageEdits.Where(e => e.ChatMessageId == messageId).ToListAsync();
    }
    
    
}
