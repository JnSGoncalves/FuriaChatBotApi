using FuriaChatBotApi.Model;
using Microsoft.Extensions.Caching.Memory;

namespace FuriaChatBotApi.Services {
    public interface ICacheService {
        Task<SessionContext> GetOrCreateContextAsync(string sessionId);
        Task SaveContextAsync(string sessionId, SessionContext context);
        Task<bool> IsSessionValidAsync(string sessionId);
    }

    public class MemoryCacheService : ICacheService {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache cache) {
            _cache = cache;
        }

        public Task<SessionContext> GetOrCreateContextAsync(string sessionId) {
            return Task.FromResult(_cache.GetOrCreate(sessionId, entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30); // Expira após 30min
                return new SessionContext { SessionId = sessionId };
            }));
        }

        public Task SaveContextAsync(string sessionId, SessionContext context) {
            _cache.Set(sessionId, context, TimeSpan.FromMinutes(30));
            return Task.CompletedTask;
        }

        public Task<bool> IsSessionValidAsync(string sessionId) {
            var exists = _cache.TryGetValue<SessionContext>(sessionId, out _);
            return Task.FromResult(exists);
        }
    }
}
