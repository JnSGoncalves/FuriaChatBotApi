using System.Text.Json;
using FuriaChatBotApi.Model;
using RestSharp;

namespace FuriaChatBotApi.Services {
    public interface IPandaScoreService {
        Task<List<TeamData>?> GetTeams(string gameSlug = null);
        Task<List<Match>?> GetMatches(string game, int match_count, bool isUpcomingSearch = false);
        Task<string[]> ListSupportedGames(string teamId);
    }
    public class PandaScoreService : IPandaScoreService {
        private readonly string _pandaScore_Token;

        public PandaScoreService() {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment != "Production") {
                DotNetEnv.Env.Load();
            }

            _pandaScore_Token = Environment.GetEnvironmentVariable("PANDA_SCORE_TOKEN");
        }

        public async Task<List<TeamData>?> GetTeams(string? gameSlug = null) {
            var url = "https://api.pandascore.co/teams?search[slug]=furia&per_page=100";

            var options = new RestClientOptions(url);
            var client = new RestClient(options);
            var request = new RestRequest("");

            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {_pandaScore_Token}");

            var response = await client.GetAsync(request);

            string json = response.Content ?? throw new Exception("Resposta nula recebida da API PandaScore");

            if (!response.IsSuccessful) {
                throw new Exception("Erro de comunicação com PandaScore");
            }

            var jsonOptions = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            var teams = JsonSerializer.Deserialize<List<TeamData>>(json, jsonOptions);

            if (!string.IsNullOrEmpty(gameSlug)) {
                var filteredTeams = teams?.Where(team => team.CurrentVideogame?.Slug == gameSlug).ToList();
                return filteredTeams;
            }

            return teams;
        }


        public async Task<List<Match>?> GetMatches(string game, int match_count, bool isUpcomingSearch = false) {
            int qtd;
            if (match_count > 1) {
                qtd = 10;
            } else {
                qtd = 1;
            }

            string setTypeMatch = isUpcomingSearch ? "upcoming" : "past";

            var urlByGame = $"https://api.pandascore.co/matches/{setTypeMatch}?per_page={qtd}&search[slug]=furia&filter[videogame]={game}";
            var urlDefault = $"https://api.pandascore.co/matches/{setTypeMatch}?per_page={qtd}&search[slug]=furia";

            var options = !string.IsNullOrEmpty(game) ? new RestClientOptions(urlByGame) : new RestClientOptions(urlDefault);

            var client = new RestClient(options);
            var request = new RestRequest("");

            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {_pandaScore_Token}");

            var response = await client.GetAsync(request);

            string json = response.Content ?? throw new Exception("Resposta nula recebida da API PandaScore");

            var jsonOptions = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            var matches = JsonSerializer.Deserialize<List<Match>>(json, jsonOptions);

            return matches;
        }

        public Task<string[]> ListSupportedGames(string teamId) {
            throw new NotImplementedException();
        }
    }
}
