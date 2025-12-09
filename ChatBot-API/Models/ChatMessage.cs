namespace ChatBot_API.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public string SessionId { get; set; } = default!;
        public string Sender { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsApproved { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime Timestamp { get; set; }
        public ICollection<MessageEdit> Edits { get; set; } = new List<MessageEdit>();
    }
}
