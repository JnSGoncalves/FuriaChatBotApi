using FuriaChatBotApi.Interface;
using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Services {
    public class ChatService : IChatService{
        private readonly ILLMService _llmService;
        private readonly WitAiService _witAiService;

        public ChatService(ILLMService llmService, WitAiService witAiService) {
            _llmService = llmService;
            _witAiService = witAiService;
        }

        public async Task<ChatResponse> GetResponseAsync(string message) {
            string? nlpResponse = await _witAiService.GetWitResponseAsync(message);

            string? answer = await _llmService.GetResponseAsync(message);

            if(answer == null) { 
                return new ChatResponse("Erro ao carregar resposta.");    
            }

            return new ChatResponse(answer);
        }
    }
}
