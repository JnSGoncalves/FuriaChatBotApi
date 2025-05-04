using FuriaChatBotApi.Helpers;
using FuriaChatBotApi.Model;
using System.Reflection;
using System.Text.Json;

namespace FuriaChatBotApi.Services {
    public interface IIntentProcessingService {
        Task<ChatResponse> ProcessIntent(RequestType request);
        Task<ChatResponse> HandleGetElenco(RequestType request, SessionContext context);
        Task<ChatResponse> HandleGetLastMatches(RequestType request, SessionContext context);
        Task<ChatResponse> HandleGetNextMatch(RequestType request, SessionContext context);
        Task<ChatResponse> HandleListSupportedGames(RequestType request, SessionContext context);
        ChatResponse HandleUnknownIntent(RequestType request, SessionContext context);
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

            if (request.Intent == "Confirm" || request.Intent == "Cancel") {
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
                    Console.WriteLine(e.Message);

                    return new ChatResponse(request.SessionId, "Erro interno.", null, ChatResponse.CodErro.InternalError);
                }
            }

            return HandleUnknownIntent(request, context) ?? new ChatResponse(
                request.SessionId, "Intent desconhecida.", null, ChatResponse.CodErro.Invalid
            );
        }

        public async Task<ChatResponse> HandleGetElenco(RequestType request, SessionContext context) {
            string msg;
            List<string>? sugestedOptions = null;
            
            if (!string.IsNullOrEmpty(request.Game)) {
                List<TeamData>? teamData = await _pandaScoreService.GetTeams(request.Game);

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
                sugestedOptions = new List<string>{
                    "Valorant",
                    "CS-GO",
                    "Rainbow Six",
                    "League of Legends"
                };
            }

            return new ChatResponse(request.SessionId, msg, sugestedOptions, ChatResponse.CodErro.Ok);
        }

        public async Task<ChatResponse> HandleGetLastMatches(RequestType request, SessionContext context) {
            string msg = "";
            List<string>? sugestedOptions = null;

            List<Match>? matches = await _pandaScoreService.GetMatches(request.Game, request.Match_count);

            if (matches != null && matches.Any()) {
                msg += "Tá na mão!\n";

                if (string.IsNullOrEmpty(request.Game)) {
                    if (request.Match_count == 0 || request.Match_count == 1) {
                        msg += $"A última partida disputada por algum time da FURIA foi:\n";
                    }else {
                        msg += $"Essas foram as últimas partidas disputadas pelos times da FURIA:\n";
                    }
                } else {
                    if (request.Match_count == 0 || request.Match_count == 1) {
                        msg += $"A última partida disputada por algum time da FURIA no {request.Game} foi:\n";
                    } else {
                        msg += $"Essas foram as últimas paritdas disputadas pelos times da FURIA no {request.Game}:\n";
                    }
                }

                msg += FormatStringMatches(matches);
            } else {
                msg = "Infelizmente, não encontrei nenhuma partida da FURIA :(\n";
            }

            msg += "Gostaria de saber mais alguma coisa sobre a FURIA ou saber quais as últimas partidas de um jogo específico?" +
                " É só perguntar!";

            sugestedOptions = new List<string>{
                    "Quais os times da FURIA no CS-GO?",
                    "Qual a próxima partida da FURIA?",
                    "Quais jogos a FURIA atua?"
                };

            return new ChatResponse(request.SessionId, msg, sugestedOptions, ChatResponse.CodErro.Ok);
        }

        public async Task<ChatResponse> HandleGetNextMatch(RequestType request, SessionContext context) {
            string msg = "";
            List<string>? sugestedOptions = null;

            List<Match>? matches = await _pandaScoreService.GetMatches(request.Game, request.Match_count, true);

            if (matches != null && matches.Any()) {
                msg += "Tá na mão!\n";

                if (string.IsNullOrEmpty(request.Game)) {
                    if (request.Match_count == 0 || request.Match_count == 1) {
                        msg += $"A próxima partida disputada pela FURIA será:\n";
                    } else {
                        msg += $"Essas são as próximas partidas que FURIA irá participar:\n";
                    }
                } else {
                    if (request.Match_count == 0 || request.Match_count == 1) {
                        msg += $"A próxima partida disputada pela FURIA no {request.Game} será:\n";
                    } else {
                        msg += $"Essas são as próximas partidas que FURIA irá participar no {request.Game}:\n";
                    }
                }

                msg += FormatStringMatches(matches, true);

            } else {
                if (string.IsNullOrEmpty(request.Game))
                    msg = "Infelizmente, não encontrei nenhuma partida agendada com os times FURIA :(\n" +
                        "Mas não se preocupe que, caso me peça mais tarde, eu posso realizar outra busca pra você!";
                else
                    msg += $"Infelizmente, não encontrei nenhuma partida de {request.Game} agendada com os times FURIA :(\n" +
                        "Mas não se preocupe que, caso me peça mais tarde, eu posso realizar outra busca pra você!";
            }

            msg += "Gostaria de saber mais alguma coisa sobre a FURIA ou saber quais as últimas partidas de um jogo específico?" +
            " É só perguntar!";

            sugestedOptions = new List<string>{
                    "Quais os times da FURIA no Valorant?",
                    "Quais as últimas partidas que a FURIA participou?",
                    "Quais jogos a FURIA atua?"
                };

            return new ChatResponse(request.SessionId, msg, sugestedOptions, ChatResponse.CodErro.Ok);
        }

        public async Task<ChatResponse> HandleListSupportedGames(RequestType request, SessionContext context) {
            string msg = "";
            List<string>? sugestedOptions = new List<string>{
                    "Quais os times da FURIA no Valorant?",
                    "Quais as últimas 3 partidas que a FURIA jogou?",
                    "Qual a próxima partida de Valorant da FURIA?"
                };

            List<TeamData>? teams = await _pandaScoreService.GetTeams(null);

            if(teams == null) {
                msg += "Infelizmente, não consegui realizar a encontrar os dados sobre a FURIA :(";
                return new ChatResponse(request.SessionId, msg, sugestedOptions, ChatResponse.CodErro.InternalError);
            }

            List<string> jogos = new List<string>();

            foreach (var team in teams) {
                if (!jogos.Contains(team.CurrentVideogame.Name)) {
                    jogos.Add(team.CurrentVideogame.Name);
                }
            }

            msg += $"Atualmente a FURIA atua no cenário de E-Sports nos seguintes jogos:\n\n";
            
            foreach (var item in jogos) {
                msg += $" - {item}\n";
            }

            return new ChatResponse(request.SessionId, msg, sugestedOptions, ChatResponse.CodErro.Ok);
        }

        public ChatResponse HandleUnknownIntent(RequestType request, SessionContext context) {
            string msg =
                "Infelizmente não entendi a sua solicitação 😔. " +
                "Atualmente possuo a capacidade de te listar as últimas ou as próximas partidas da FURIA, " +
                "além de te mostrar quais os times da FURIA.\n\n" +
                "Caso queira saber algo desse tipo é só me perguntar!";

            List<string>? sugestedOptions = new List<string>{
                    "Quais os times da FURIA no CS-GO?",
                    "Qual a próxima partida de Valorant da FURIA?",
                    "Quais as últimas 3 partidas que a FURIA jogou?",
                    "Quais jogos a FURIA atua?"
                };

            return new ChatResponse(request.SessionId, msg, sugestedOptions, ChatResponse.CodErro.Invalid);
        }

        private string FormatStringMatches(List<Match> matches, bool isNextMatches = false) {
            string msg = "\n";

            foreach (Match match in matches) {
                msg +=
                    $" - {match.Name} pela {match.League.Name}\n" +
                    $"Onde {match.Opponents.First().Opponent.Name} e {match.Opponents[1].Opponent.Name} se ";
                msg += isNextMatches ? "enfrentarão!" : $"enfrentaram!\n";

                if (!isNextMatches && match.Winner != null) {
                    if (match.Winner.Slug.Contains("furia")) {
                        msg += $"Nessa partida {match.Winner.Name} saiu com a vitória! Ganhando por ";
                    } else {
                        msg += $"Infelizmente perdemos para o time {match.Winner.Name}, que foi um ótimo oponente! " +
                            $"Levando a vitória por ";
                    }

                    if (match.Results.First().TeamId == match.Winner.Id) {
                        msg += $"{match.Results.First().Score} a {match.Results[1].Score}\n";
                    } else {
                        msg += $"{match.Results[1].Score} a {match.Results.First().Score}\n";
                    }

                    msg += $"A partida ocorreu {DateTimeHelpers.FormatBrasiliaTime(match.BeginAt)}.\n";
                } else {
                    msg += $"A partida ocorrerá {DateTimeHelpers.FormatBrasiliaTime(match.BeginAt)}.";
                }

                msg += $"\n";
            }

            return msg;
        }
    }
}
