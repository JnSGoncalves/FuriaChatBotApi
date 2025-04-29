
using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Services {
    public interface IChatService {
        Task<ChatResponse> GetResponseAsync(string sessionId, string message);
    }

    public class ChatService : IChatService {
        private readonly WitAiService _witAiService;
        private readonly IIntentProcessingService _intentProcessingService;

        public ChatService(WitAiService witAiService, IIntentProcessingService intentProcessingService) {
            _witAiService = witAiService;
            _intentProcessingService = intentProcessingService;
        }

        public async Task<ChatResponse> GetResponseAsync(string sessionId, string message) {
            var witJson = await _witAiService.GetWitResponseAsync(message);
            Console.WriteLine(witJson);

            RequestType request = RequestType.GetRequestFromWitJson(sessionId, witJson);

            Console.WriteLine($"SessionID: {request.SessionId}");
            Console.WriteLine($"Intent: {request.Intent}");
            Console.WriteLine($"Game: {request.Game}");
            Console.WriteLine($"Match_Count: {request.Match_count}");
            Console.WriteLine($"Match_type: {request.Match_type}");
            
            string info = await _intentProcessingService.ProcessIntent(request);

            Console.WriteLine(info);

            return new ChatResponse(sessionId, info, null);
        }
    }
}
