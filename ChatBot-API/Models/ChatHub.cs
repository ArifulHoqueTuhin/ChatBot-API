using Microsoft.AspNetCore.SignalR;

namespace ChatBot_API.Models
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
