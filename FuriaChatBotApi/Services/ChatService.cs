using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Services {
    /// <summary>
    /// Serviço de interpretação do Chat
    /// </summary>
    public interface IChatService {
        /// <summary>
        /// Método de obtenção da resposta da API para retorno
        /// </summary>
        /// <param name="sessionId">Id da sessão</param>
        /// <param name="message">Mensagem enviada pelo usuário</param>
        /// <returns>Retorno do modelo de resposta formalizado</returns>
        Task<ChatResponse> GetResponseAsync(string sessionId, string message);
    }

    public class ChatService : IChatService {
        private readonly WitAiService _witAiService;
        private readonly IIntentProcessingService _intentProcessingService;
        private readonly ICacheService _cacheService;

        public ChatService(WitAiService witAiService, IIntentProcessingService intentProcessingService, ICacheService cacheService) {
            _witAiService = witAiService;
            _intentProcessingService = intentProcessingService;
            _cacheService = cacheService;
        }

        public async Task<ChatResponse> GetResponseAsync(string sessionId, string message) {
            var witJson = await _witAiService.GetWitResponseAsync(message);
            Console.WriteLine(witJson);

            RequestType request = RequestType.GetRequestFromWitJson(sessionId, witJson);

            Console.WriteLine($"SessionID: {request.SessionId}");
            Console.WriteLine($"Intent: {request.Intent}");
            Console.WriteLine($"Game: {request.Game}");
            Console.WriteLine($"Match_Count: {request.Match_count}");
            Console.WriteLine($"Match_type: {request.Match_type}\n\n");

            ChatResponse info = await _intentProcessingService.ProcessIntent(request);

            if (info.Status != ChatResponse.CodErro.Ok) {
                return info;
            }

            SessionContext context = new SessionContext(sessionId);
            context.CurrentStep = request.Intent;
            context.LastEntites = new LastEntites {
                Game = request.Game,
                Match_count = request.Match_count,
                Match_type = request.Match_type
            };

            await _cacheService.SaveContextAsync(sessionId, context);

            return info;
        }
    }
}
