namespace FuriaChatBotApi.Model {
    /// <summary>
    /// Modelo de resposta da API
    /// </summary>
    public class ChatResponse {
        public enum CodErro {
            Ok,        /// Operação bem-sucedida
            NotImplemented, /// Intenção não implementada
            Invalid,         /// Intenção inválida ou desconhecida
            InternalError
        }

        public CodErro Status { get; set; }
        public string SessionId { get; set; }
        public string Reply { get; set; }
        public List<string>? SugestedOptions { get; set; }

        public ChatResponse() { }

        public ChatResponse(string sessionId, string message, List<string> sugestedOptions, CodErro codErro) {
            SessionId = sessionId;
            Reply = message;
            SugestedOptions = sugestedOptions;
            Status = codErro;
        }
    }
}
