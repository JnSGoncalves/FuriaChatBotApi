using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace FuriaChatBotApi.Model {
    public class WitEntityValue {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class WitEntities {
        [JsonPropertyName("game:game")]
        public List<WitEntityValue> Game { get; set; }

        [JsonPropertyName("match_count:match_count")]
        public List<WitEntityValue> MatchCount { get; set; }

        [JsonPropertyName("match_type:match_type")]
        public List<WitEntityValue> MatchType { get; set; }
    }

    public class WitIntent {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class WitAiResponse {
        [JsonPropertyName("entities")]
        public WitEntities Entities { get; set; }

        [JsonPropertyName("intents")]
        public List<WitIntent> Intents { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
