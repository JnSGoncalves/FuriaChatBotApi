using FuriaChatBotApi.Model;

namespace FuriaChatBotApi.Interface {
    public interface IIntentProcessingService {
        string ProcessIntent(RequestType request);
    }
}
