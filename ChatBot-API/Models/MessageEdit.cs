namespace ChatBot_API.Models
{
    public class MessageEdit : BaseEntity
    {
        public int ChatMessageId { get; set; }
        public ChatMessage ChatMessage { get; set; } = new();
        public string PreviousMessage { get; set; } = default!;
    }
}
