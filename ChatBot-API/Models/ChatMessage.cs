namespace ChatBot_API.Models
{
    public class ChatMessage : BaseEntity
    {
        public string UserId { get; set; } = default!;
        public string SessionId { get; set; } = default!;
        public string Sender { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsApproved { get; set; } = true;
        public ICollection<MessageEdit> Edits { get; set; } = new List<MessageEdit>();
    }
}
