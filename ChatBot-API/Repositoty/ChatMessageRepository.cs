using ChatBot_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBot_API.Repositoty
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatMessage>> GetAllAsync() => await _context.ChatMessages.ToListAsync();

        public async Task<ChatMessage> GetByIdAsync(int id) => await _context.ChatMessages.FindAsync(id);

        public async Task AddAsync(ChatMessage entity) => await _context.ChatMessages.AddAsync(entity);

        public void Update(ChatMessage entity) => _context.ChatMessages.Update(entity);

        public void Delete(ChatMessage entity) => _context.ChatMessages.Remove(entity);

        public async Task<IEnumerable<ChatMessage>> GetMessagesByUserAsync(string userId) =>
            await _context.ChatMessages.Where(m => m.UserId == userId).ToListAsync();

        //public async Task<IEnumerable<ChatMessage>> GetMessagesBySessionAsync(string sessionId) =>
        //    await _context.ChatMessages.Where(m => m.SessionId == sessionId).ToListAsync();


        public async Task<IEnumerable<ChatMessage>> GetPaginatedMessagesAsync(string userId, int page, int pageSize)
            => await _context.ChatMessages
                .Where(m => m.UserId == userId && !m.IsDeleted)
                .Include(m => m.Edits)
                .OrderByDescending(m => m.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();




        public async Task<ChatMessage?> GetBotReplyForUserMessage(ChatMessage userMessage)
        {
            return await _context.ChatMessages
                .Where(m => m.SessionId == userMessage.SessionId &&
                            m.UserId == userMessage.UserId &&
                            m.Sender == "bot" &&
                            m.Timestamp > userMessage.Timestamp)
                .OrderBy(m => m.Timestamp)
                .FirstOrDefaultAsync();
        }



    }
}

