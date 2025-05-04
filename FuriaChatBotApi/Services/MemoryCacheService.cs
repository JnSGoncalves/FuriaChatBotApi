using FuriaChatBotApi.Model;
using Microsoft.Extensions.Caching.Memory;

namespace FuriaChatBotApi.Services {
    /// <summary>
    /// Serviço responsavem pelo gerenciamento do cache das sessões
    /// </summary>
    public interface ICacheService {
        /// <summary>
        /// Método de obtenção ou criação de um contexto de sessão na memória cache
        /// </summary>
        /// <param name="sessionId">Id da sessão</param>
        /// <returns>Contexto de sessão</returns>
        Task<SessionContext> GetOrCreateContextAsync(string sessionId);
        /// <summary>
        /// Salva as alteração de um contexto de sessão
        /// </summary>
        /// <param name="sessionId">Id da sessão</param>
        /// <param name="context">Contexto da sessão</param>
        /// <returns>Contexto de sessão passado como parametro</returns>
        Task SaveContextAsync(string sessionId, SessionContext context);
        /// <summary>
        /// Método de validação do de um Id de Sessão
        /// </summary>
        /// <param name="sessionId">Id da sessão</param>
        /// <returns>True = Id válido | False = Id inválido</returns>
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
