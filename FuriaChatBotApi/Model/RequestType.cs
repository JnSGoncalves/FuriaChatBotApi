using System.Text.Json;
using System.Text.RegularExpressions;

namespace FuriaChatBotApi.Model {
    /// <summary>
    /// Modelo de requisição para ser utilizada de base pelo código
    /// </summary>
    public class RequestType {
        public string SessionId { get; set; }
        public string Intent { get; set; }
        public string Game { get; set; }
        public int Match_count { get; set; }
        public string Match_type { get; set; }

        public RequestType(string sessionId, string intent, string game, int match_count, string match_type) {
            SessionId = sessionId;
            Intent = intent;
            this.Game = game;
            this.Match_count = match_count;
            this.Match_type = match_type;
        }


        /// <summary>
        /// Fabrica do modelo de Request com base no json de retorno do modelo NLP do Wit.ai
        /// </summary>
        /// <param name="sessionId">Id da sessão.</param>
        /// <param name="json">Json de retorno do Wit.ai como string.</param>
        /// <returns>RequestType formalizado.</returns>
        public static RequestType GetRequestFromWitJson(string sessionId, string json) {
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            WitAiResponse? witResponse = JsonSerializer.Deserialize<WitAiResponse>(json, options);

            if(witResponse == null) {
                return new RequestType(sessionId,"Error Intent", "", 1, "last");
            }

            var intentObj = SafeGetFirstOrDefault(witResponse.Intents);
            var gameObj = SafeGetFirstOrDefault(witResponse.Entities.Game);
            var matchCountObj = SafeGetFirstOrDefault(witResponse.Entities.MatchCount);
            var matchTypeObj = SafeGetFirstOrDefault(witResponse.Entities.MatchType);

            string intent = intentObj?.Name ?? "";
            string game = gameObj?.Value ?? "";
            string matchCountStr = matchCountObj?.Value ?? "";
            string matchTypeStr = matchTypeObj?.Value ?? "";

            int matchCount = ExtractNumberFromText(matchCountStr);
            string matchType = string.IsNullOrEmpty(matchTypeStr) ? "last" : matchTypeStr;

            return new RequestType(sessionId, intent, game, matchCount, matchType);
        }

        /// <summary>
        /// Função que extrai o número de uma string, seja por número escrito ou por extenso
        /// </summary>
        /// <param name="text">string para coleta do número.</param>
        private static int ExtractNumberFromText(string text) {
            if (string.IsNullOrEmpty(text))
                return 0;

            System.Text.RegularExpressions.Match match = Regex.Match(text, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int number)) {
                return number;
            }

            // Caso não tenha encontrado um número, tenta interpretar números por extenso
            var numberWords = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "um", 1 },
                { "uma", 1 },
                { "dois", 2 },
                { "duas", 2 },
                { "três", 3 },
                { "tres", 3 },
                { "quatro", 4 },
                { "cinco", 5 },
                { "seis", 6 },
                { "sete", 7 },
                { "oito", 8 },
                { "nove", 9 },
                { "dez", 10 },
                { "onze", 11 },
                { "doze", 12 },
                { "treze", 13 },
                { "quatorze", 14 },
                { "catorze", 14 },
                { "quinze", 15 },
                { "dezesseis", 16 },
                { "dezessete", 17 },
                { "dezoito", 18 },
                { "dezenove", 19 },
                { "vinte", 20 }
            };

            foreach (var word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
                string cleanWord = word.ToLower().Normalize(System.Text.NormalizationForm.FormD);
                cleanWord = new string(cleanWord.Where(c => char.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark).ToArray());

                if (numberWords.TryGetValue(cleanWord, out int value)) {
                    return value;
                }
            }

            return 1;
        }

        private static T? SafeGetFirstOrDefault<T>(List<T> list) where T : class {
            return (list != null && list.Count > 0) ? list[0] : null;
        }
    }
}