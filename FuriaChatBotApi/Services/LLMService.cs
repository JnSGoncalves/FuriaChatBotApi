using FuriaChatBotApi.Configs;
using System.Net.Http;
using System.Runtime;
using FuriaChatBotApi.Interface;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Services {
    public class LLMService : ILLMService {
        // Atualmente utilizando Gemini AI

        private readonly HttpClient _httpClient;
        private readonly GeminiSettings _settings;

        public LLMService(HttpClient httpClient, IOptions<GeminiSettings> settings) {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<ChatResponse?> GetResponseAsync(string prompt) {
            var body = new {
                contents = new[] {
                    new {
                        parts = new[] { new { text = prompt } }
                    }
                }
            };

            var request = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_settings.Endpoint}?key={_settings.ApiKey}"),
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var root = JsonDocument.Parse(json).RootElement;

            var answer = root
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return answer != null ? new ChatResponse(answer) : null;
        }
    }
}
