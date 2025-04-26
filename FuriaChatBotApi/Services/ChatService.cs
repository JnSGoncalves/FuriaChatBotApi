using FuriaChatBotApi.Interface;
using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Services {
    public class ChatService : IChatService{
        private readonly ILLMService _llmService;

        public ChatService(ILLMService llmService) {
            _llmService = llmService;
        }

        public async Task<ChatResponse> GetResponseAsync(string message) {
            string? answer = await _llmService.GetResponseAsync(message);

            if(answer == null) { 
                return new ChatResponse("Erro ao carregar resposta.");    
            }

            return new ChatResponse(answer);
        }
    }
}
