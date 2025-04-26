namespace FuriaChatBotApi.Model {
    public class ChatRequest {
        public string Message { get; set; }

        //public ChatRequest() { }

        public ChatRequest(string message) {
            this.Message = message;
        }
    }
}
