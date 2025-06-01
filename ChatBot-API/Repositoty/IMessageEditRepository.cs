using ChatBot_API.Models;

namespace ChatBot_API.Repositoty
{
    
    
        public interface IMessageEditRepository : IRepository<MessageEdit>
        {
            Task<IEnumerable<MessageEdit>> GetEditsByMessageIdAsync(int messageId);
        }
 }

