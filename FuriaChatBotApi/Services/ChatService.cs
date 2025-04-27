using FuriaChatBotApi.Interface;
using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Services {
    public class ChatService : IChatService{
        private readonly ILLMService _llmService;
        private readonly WitAiService _witAiService;
        private readonly IIntentProcessingService _intentProcessingService;

        public ChatService(ILLMService llmService, WitAiService witAiService, IIntentProcessingService intentProcessingService) {
            _llmService = llmService;
            _witAiService = witAiService;
            _intentProcessingService = intentProcessingService;
        }

        public async Task<ChatResponse> GetResponseAsync(string message) {
            var witJson = await _witAiService.GetWitResponseAsync(message);
            //Console.WriteLine(witJson);

            RequestType request = RequestType.GetRequestFromWitJson(witJson);
 
            Console.WriteLine($"Intent: {request.Intent}");
            Console.WriteLine($"Game: {request.Game}");
            Console.WriteLine($"Match_Count: {request.Match_count}");
            Console.WriteLine($"Match_type: {request.Match_type}");


            string prompt = "Interprete um ChatBot que responde sobre o time de E-sports Furia. " +
                $"Mensagem de contexto enviada pelo usuário: {message}. Dados para a resposta: ";
            prompt += _intentProcessingService.ProcessIntent(request);

            return new ChatResponse(prompt);

            //string? answer = await _llmService.GetResponseAsync(prompt);

            //if (answer == null) {
            //    return new ChatResponse("Erro ao carregar resposta.");
            //}

            //return new ChatResponse(answer);
        }
    }
}
