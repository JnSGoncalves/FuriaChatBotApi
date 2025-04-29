namespace FuriaChatBotApi.Model {
    public class ChatResponse {
        public string SessionId { get; set; }
        public string Reply { get; set; }
        public List<string>? SugestedOptions { get; set; }

        public ChatResponse(string sessionId, string message, List<string> sugestedOptions) {
            SessionId = sessionId;
            Reply = message;
            SugestedOptions = sugestedOptions;
        }
    }
}
