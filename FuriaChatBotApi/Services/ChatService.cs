using FuriaChatBotApi.Interface;
using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Services {
    public class ChatService : IChatService{
        public ChatService() { }

        public Task<ChatResponse> GetResponseAsync(string message) {
            throw new NotImplementedException();
        }
    }
}
