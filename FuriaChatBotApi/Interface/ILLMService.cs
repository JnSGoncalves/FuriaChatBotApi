using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Interface {
    public interface ILLMService {
        Task<string?> GetResponseAsync(string prompt);
    }
}
