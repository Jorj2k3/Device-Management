using System.Text;
using System.Text.Json;

namespace DeviceManagement.Api.Services
{
    public class GeminiAiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public GeminiAiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> GenerateDeviceDescriptionAsync(string name, string manufacturer, string os, string type, int ram, string processor)
        {
            var apiKey = _config["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey)) return "Error: AI API Key is missing in appsettings.json!";

            var prompt = $"Generate a human-readable, concise, and informative description of a device based on its technical specifications, no more than one sentence," +
                $"and do not be too technical, tell it in a way that a general audience can understand, without saying its specifications." +
                $"Input: Name - {name}, Manufacturer - {manufacturer}, OS - {os}, Type - {type}, RAM - {ram}GB, Processor - {processor}." +
                $"Output only the description as a single sentence without quotes.";

            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = prompt } } } }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}", content);

            if (!response.IsSuccessStatusCode) return "Error: Failed to connect to AI Service.";

            var responseString = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseString);

            var description = jsonDoc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text").GetString();

            return description?.Trim() ?? "Description could not be generated.";
        }
    }
}
