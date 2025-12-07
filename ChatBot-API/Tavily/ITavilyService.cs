namespace ChatBot_API.Tavily
{
    public interface ITavilyService
    {
        Task<string> GetBotResponseAsync(string userMessage);
    }
}
