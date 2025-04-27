using System.Text.Json;
using System.Text.RegularExpressions;

namespace FuriaChatBotApi.Model {
    public class RequestType {
        public string Intent { get; set; }
        public string Game { get; set; }
        public int Match_count { get; set; }
        public string Match_type { get; set; }

        public RequestType(string intent, string game, int match_count, string match_type) {
            Intent = intent;
            this.Game = game;
            this.Match_count = match_count;
            this.Match_type = match_type;
        }

        public static RequestType GetRequestFromWitJson(string json) {
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            WitAiResponse witResponse = JsonSerializer.Deserialize<WitAiResponse>(json, options);

            string intent = witResponse.Intents?[0]?.Name ?? "";
            string game = witResponse.Entities?.Game?[0]?.Value ?? "";
            string matchCountStr = witResponse.Entities?.MatchCount?[0]?.Value ?? "";
            string matchTypeStr = witResponse.Entities?.MatchType?[0]?.Value ?? "";

            int matchCount = ExtractNumberFromText(matchCountStr);
            string matchType = string.IsNullOrEmpty(matchTypeStr) ? "last" : matchTypeStr;

            return new RequestType(intent, game, matchCount, matchType);
        }

        // Função que extrai o número de uma string, seja por número escrito ou por extenso
        private static int ExtractNumberFromText(string text) {
            if (string.IsNullOrEmpty(text))
                return 0;

            Match match = Regex.Match(text, @"\d+");
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
                { "tres", 3 }, // sem acento
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
    }
}