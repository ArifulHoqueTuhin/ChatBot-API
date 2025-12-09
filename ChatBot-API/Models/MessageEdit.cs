namespace ChatBot_API.Models
{
    public class MessageEdit
    {
        public int Id { get; set; }
        public int ChatMessageId { get; set; }
        public ChatMessage ChatMessage { get; set; } = new();
        public string PreviousMessage { get; set; } = default!;
        public DateTime EditedAt { get; set; }
    }
}
