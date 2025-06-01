namespace ChatBot_API.Models
{
    public class MessageEdit
    {
        public int Id { get; set; }
        public int ChatMessageId { get; set; }
        public ChatMessage ChatMessage { get; set; } 

        public string PreviousMessage { get; set; }
        public DateTime EditedAt { get; set; }
    }
}
