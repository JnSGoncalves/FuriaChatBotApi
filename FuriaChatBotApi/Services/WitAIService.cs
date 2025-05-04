using System.Net.Http.Headers;
using FuriaChatBotApi.Model;
using System.Text.Json;

namespace FuriaChatBotApi.Services {
    /// <summary>
    /// Serviço responsável pela comunicação com o modelo NLP na Wit.ai
    /// </summary>
    public class WitAiService {
        private readonly HttpClient _httpClient;
        private readonly string _witToken;

        public WitAiService(HttpClient httpClient) {
            _httpClient = httpClient;

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment != "Production") {
                DotNetEnv.Env.Load();
            }

            _witToken = Environment.GetEnvironmentVariable("WIT_AI_TOKEN");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _witToken);
        }

        /// <summary>
        /// Método que realiza a requisição da resposta da Wit.ai com base na mensagem do usuário
        /// </summary>
        /// <param name="message">Mensagem do usuário</param>
        /// <returns>String do Json de resposta do modelo</returns>
        public async Task<string> GetWitResponseAsync(string message) {
            var url = $"https://api.wit.ai/message?v=20240426&q={Uri.EscapeDataString(message)}";
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}