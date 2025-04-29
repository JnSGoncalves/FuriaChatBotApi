using FuriaChatBotApi.Model;
using System.Reflection;
using System.Text.Json;

namespace FuriaChatBotApi.Services {
    public interface IIntentProcessingService {
        Task<ChatResponse> ProcessIntent(RequestType request);
        Task<ChatResponse> HandleGetElenco(RequestType request, SessionContext context);
        ChatResponse HandleGetGeneralInfo(RequestType request, SessionContext context);
        ChatResponse HandleGetLastMatches(RequestType request, SessionContext context);
        ChatResponse HandleGetNextMatch(RequestType request, SessionContext context);
        ChatResponse HandleListSupportedGames(RequestType request, SessionContext context);
        ChatResponse? HandleUnknownIntent(RequestType request, SessionContext context);
    }

    public class IntentProcessingService : IIntentProcessingService {
        private readonly PandaScoreService _pandaScoreService;
        private readonly ICacheService _cacheService;

        public IntentProcessingService(PandaScoreService pandaScoreService, ICacheService cacheService) {
            _pandaScoreService = pandaScoreService;
            _cacheService = cacheService;
        }

        public async Task<ChatResponse> ProcessIntent(RequestType request) {
            SessionContext context = await _cacheService.GetOrCreateContextAsync(request.SessionId);

            if (string.IsNullOrEmpty(request.Intent) || 
                        request.Intent == "Confirm" || 
                        request.Intent == "Cancel") {
                request.Intent = context.CurrentStep;
            }

            Console.WriteLine($"Contexto atual:\n" +
                $"SessionId={context.SessionId}\n" +
                $"CurrentStep={context.CurrentStep}\n" +
                $"LastInteraction={context.LastInteraction}\n" +
                $"LastEntites={JsonSerializer.Serialize(context.LastEntites)}\n\n");

            string methodName = "Handle" + request.Intent;
            MethodInfo method = typeof(IntentProcessingService).GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
            );

            if (method != null) {
                try {
                    // Verifica se o método é assíncrono (Task<ChatResponse>)
                    if (method.ReturnType == typeof(Task<ChatResponse>)) {
                        var task = method.Invoke(this, new object[] { request, context }) as Task<ChatResponse>;
                        return await task;
                    } else {
                        // Método síncrono (ChatResponse)
                        var result = method.Invoke(this, new object[] { request, context }) as ChatResponse;
                        return result ?? throw new InvalidOperationException("Resposta nula.");
                    }
                } catch (Exception e) {
                    return new ChatResponse(request.SessionId, "Erro interno.", null, ChatResponse.CodErro.InternalError);
                }
            }

            return HandleUnknownIntent(request, context) ?? new ChatResponse(
                request.SessionId, "Intent desconhecida.", null, ChatResponse.CodErro.Invalid
            );
        }

        public async Task<ChatResponse> HandleGetElenco(RequestType request, SessionContext context) {
            string msg;
            List<string>? sugestOptions = null;
            
            if (!string.IsNullOrEmpty(request.Game)) {
                List<TeamData> teamData = await _pandaScoreService.GetTeamsByGame(request.Game);

                msg = "";
                if (teamData != null && teamData.Any()) {
                    msg = teamData.Count > 1 ?
                        $"Esses são os atuais times da FURIA no {request.Game}:\n\n":
                        $"Esses é o atual time da FURIA no {request.Game}:\n\n";

                    foreach (var team in teamData) {
                        msg += $"- Equipe {team.Name}.\n";
                        if (team.Players != null && team.Players.Any()) {
                            foreach (var player in team.Players) {
                                msg += $"    - {player.Name} ({player.FirstName} {player.LastName})\n";
                            }
                        } else {
                            msg += "  Sem jogadores cadastrados.\n";
                        }
                        msg += "\n";
                    }
                } else {
                    msg = "Não encontrei nenhum time da FURIA atuando nesse jogo :(";
                }
            } else {
                msg = 
                    "Quer saber quem são os craques que jogam nos time de E-Sport da FURIA?\n" +
                    "Então me diz aí, de qual jogo você quer saber?";
                sugestOptions = new List<string>{
                    "Valorant",
                    "CS-GO",
                    "Rainbow Six",
                    "League of Legends"
                };
            }

            return new ChatResponse(request.SessionId, msg, sugestOptions, ChatResponse.CodErro.Ok);
        }

        public ChatResponse HandleGetGeneralInfo(RequestType request, SessionContext context) {
            throw new NotImplementedException();
        }

        public ChatResponse HandleGetLastMatches(RequestType request, SessionContext context) {
            throw new NotImplementedException();
        }

        public ChatResponse HandleGetNextMatch(RequestType request, SessionContext context) {
            throw new NotImplementedException();
        }

        public ChatResponse HandleListSupportedGames(RequestType request, SessionContext context) {
            throw new NotImplementedException();
        }

        public ChatResponse? HandleUnknownIntent(RequestType request, SessionContext context) {
            return null;
        }
    }
}
