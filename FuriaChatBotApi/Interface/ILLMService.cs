using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Interface {
    public interface ILLMService {
        Task<ChatResponse?> GetResponseAsync(string prompt);
    }
}
