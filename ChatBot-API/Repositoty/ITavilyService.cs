namespace ChatBot_API.Repositoty
{
    public interface ITavilyService
    {
        Task<string> GetBotResponseAsync(string userMessage);
    }
}
