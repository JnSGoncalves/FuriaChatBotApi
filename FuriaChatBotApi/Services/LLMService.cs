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

        public async Task<string?> GetResponseAsync(string prompt) {
            var body = new {
                contents = new[] {
                    new {
                            parts = new[]
                        {
                            new { text = prompt }
                        }
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

            // Verifica se existe o campo de texto da resposta do Gemini
            if (root.TryGetProperty("candidates", out var candidates)) {
                if (candidates.GetArrayLength() > 0) {
                    var candidate = candidates[0];
                    if (candidate.TryGetProperty("content", out var content)) {
                        if (content.TryGetProperty("parts", out var parts)) {
                            if (parts.GetArrayLength() > 0) {
                                var part = parts[0];
                                if (part.TryGetProperty("text", out var text)) {
                                    return text.GetString();
                                }
                            }
                        }
                    }
                }
            }

            // Se não encontrou a resposta esperada, loga o JSON (pra debug) e retorna null
            Console.WriteLine("Resposta inesperada do Gemini:");
            Console.WriteLine(json);

            return null;
        }
    }
}
