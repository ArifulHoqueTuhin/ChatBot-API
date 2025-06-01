namespace ChatBot_API.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string Sender { get; set; } 
        public string Message { get; set; }
        public bool IsApproved { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime Timestamp { get; set; }

        public ICollection<MessageEdit> Edits { get; set; }
    }
}
