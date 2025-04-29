using FuriaChatBotApi.Model;
using System.Reflection;
using System.Text.Json;

namespace FuriaChatBotApi.Services {
    public interface IIntentProcessingService {
        Task<string> ProcessIntent(RequestType request);
        Task<string> HandleGetElenco(RequestType request, SessionContext context);
        string HandleGetGeneralInfo(RequestType request, SessionContext context);
        string HandleGetLastMatches(RequestType request, SessionContext context);
        string HandleGetNextMatch(RequestType request, SessionContext context);
        string HandleListSupportedGames(RequestType request, SessionContext context);
        string? HandleUnknownIntent(RequestType request, SessionContext context);
    }

    public class IntentProcessingService : IIntentProcessingService {
        private readonly PandaScoreService _pandaScoreService;
        private readonly ICacheService _cacheService;

        public IntentProcessingService(PandaScoreService pandaScoreService, ICacheService cacheService) {
            _pandaScoreService = pandaScoreService;
            _cacheService = cacheService;
        }

        public async Task<string> ProcessIntent(RequestType request) {
            string methodName = "Handle" + request.Intent;
            MethodInfo? method = typeof(IntentProcessingService).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

            SessionContext context = await _cacheService.GetOrCreateContextAsync(request.SessionId);

            if (method != null) {
                // Verifica se o método é assíncrono
                if (method.ReturnType == typeof(Task<string>)) {
                    // Invoca o método assíncrono
                    var result = method.Invoke(this, new object[] { request, context }) as Task<string>;
                    if (result != null) {
                        return await result;
                    }
                } else {
                    // Invoca o método síncrono
                    var result = method.Invoke(this, new object[] { request, context }) as string;
                    if (result != null) {
                        return result;
                    }
                }
            }

            return HandleUnknownIntent(request, context) ?? "Intent desconhecida.";
        }

        public async Task<string> HandleGetElenco(RequestType request, SessionContext context) {
            List<TeamData> teamData = await _pandaScoreService.GetTeamsByGame(request.Game);

            string jsonResponse = JsonSerializer.Serialize(teamData);

            return $"Esses são os dados dos times da Furia para o jogo {request.Game}: \n{jsonResponse}\n" +
                $"Responda listando os times e os nomes e nicks de seus jogadores";
        }

        public string HandleGetGeneralInfo(RequestType request, SessionContext context) {
            return $"Exibindo informações gerais sobre o jogo {request.Game}";
        }

        public string HandleGetLastMatches(RequestType request, SessionContext context) {
            return $"Exibindo as {request.Match_count} últimas partidas do jogo {request.Game}";
        }

        public string HandleGetNextMatch(RequestType request, SessionContext context) {
            return $"Exibindo a próxima partida do jogo {request.Game}";
        }

        public string HandleListSupportedGames(RequestType request, SessionContext context) {
            return "Exibindo os jogos suportados";
        }

        public string? HandleUnknownIntent(RequestType request, SessionContext context) {
            return null;
        }
    }
}
