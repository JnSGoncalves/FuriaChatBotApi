using System.Net.Http.Headers;

namespace FuriaChatBotApi.Services {
    public class WitAiService {
        private readonly HttpClient _httpClient;
        private readonly string _witToken;

        public WitAiService(HttpClient httpClient) {
            _httpClient = httpClient;

            DotNetEnv.Env.Load();
            _witToken = Environment.GetEnvironmentVariable("WIT_AI_TOKEN");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _witToken);
        }

        public async Task<string> GetWitResponseAsync(string message) {
            var url = $"https://api.wit.ai/message?v=20240426&q={Uri.EscapeDataString(message)}";
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseBody);

            return responseBody;
        }
    }
}