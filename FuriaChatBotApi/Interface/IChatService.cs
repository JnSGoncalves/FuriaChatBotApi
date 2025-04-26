using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Interface {
    public interface IChatService {
        Task<ChatResponse> GetResponseAsync(string message);
    }
}
