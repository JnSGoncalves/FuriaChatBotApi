using FuriaChatBotApi.Interface;
using FuriaChatBotApi.Model;
using System.Reflection;

namespace FuriaChatBotApi.Services {
    public class IntentProcessingService : IIntentProcessingService {
        public string ProcessIntent(RequestType request) {
            string methodName = "Handle" + request.Intent;
            MethodInfo method = typeof(IntentProcessingService).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

            if (method != null) {
                return (string)method.Invoke(this, new object[] { request });
            } else {
                return HandleUnknownIntent(request);
            }
        }

        public string HandleGetElenco(RequestType request) {
            if (request.Game == "") {


            }

            return $"Exibindo o elenco do jogo {request.Game}";
        }

        public string HandleGetGeneralInfo(RequestType request) {
            return $"Exibindo informações gerais sobre o jogo {request.Game}";
        }

        public string HandleGetLastMatches(RequestType request) {
            return $"Exibindo as {request.Match_count} últimas partidas do jogo {request.Game}";
        }

        public string HandleGetNextMatch(RequestType request) {
            return $"Exibindo a próxima partida do jogo {request.Game}";
        }

        public string HandleListSupportedGames(RequestType request) {
            return "Exibindo os jogos suportados";
        }

        public string HandleUnknownIntent(RequestType request) {
            return "Intent desconhecida. Não foi possível processar.";
        }
    }
}
