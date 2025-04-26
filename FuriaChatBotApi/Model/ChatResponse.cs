namespace FuriaChatBotApi.Model {
    public class ChatResponse {
        public string Reply { get; set; }

        public ChatResponse(string message) {
            Reply = message;
        }
    }
}
