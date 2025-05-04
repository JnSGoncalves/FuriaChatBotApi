namespace FuriaChatBotApi.Model {
    /// <summary>
    /// Modelo de requisição para utilização da API
    /// </summary>
    public class ChatRequest {
        public string SessionId { get; set; }
        public string Message { get; set; }

        public ChatRequest(string sessionId, string message) {
            SessionId = sessionId;
            this.Message = message;
        }
    }
}
