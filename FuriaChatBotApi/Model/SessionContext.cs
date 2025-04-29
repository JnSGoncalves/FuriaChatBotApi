namespace FuriaChatBotApi.Model {
    public class SessionContext {
        public string SessionId { get; set; }
        public string CurrentStep { get; set; }
        public LastEntites LastEntites { get; set; }
        public DateTime LastInteraction { get; set; } = DateTime.UtcNow;

        public SessionContext() { }

        public SessionContext(string sessionId) { SessionId = sessionId; }
    }

    public class LastEntites {
        public string Game { get; set; }
        public int Match_count { get; set; }
        public string Match_type { get; set; }
    }
}
