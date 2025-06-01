using ChatBot_API.Models;

namespace ChatBot_API.Repositoty
{
    
    
        public interface IChatMessageRepository : IRepository<ChatMessage>
        {
            Task<IEnumerable<ChatMessage>> GetMessagesByUserAsync(string userId);
            Task<IEnumerable<ChatMessage>> GetMessagesBySessionAsync(string sessionId);

            Task<IEnumerable<ChatMessage>> GetPaginatedMessagesAsync(string userId, int page, int pageSize);
    }
}
    

