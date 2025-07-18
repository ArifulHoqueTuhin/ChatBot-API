
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace ChatBot_API.Repositoty
{
    public class TavilyService : ITavilyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TavilyService (HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiSettings:TavilyApiKey"];
        }

        public async Task<string> GetBotResponseAsync(string userMessage)
        {
            
            var requestBody = new
            {
                query = userMessage,
                search_depth = "advanced", 
                include_answer = true
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

           
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            try
            {
                var response = await _httpClient.PostAsync("https://api.tavily.com/search", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Tavily API Error: {error}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                //Console.WriteLine($"Tavily Raw JSON: {responseContent}");

               
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                
                if (root.TryGetProperty("answer", out JsonElement answerElement))
                {
                    return answerElement.GetString();
                }

               
                return "Sorry, I couldn't generate a response.";
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Tavily API Exception: {ex.Message}");
                return "Sorry, something went wrong while contacting Tavily AI.";
            }
        }
    }
}
