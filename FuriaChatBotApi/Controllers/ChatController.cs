using FuriaChatBotApi.Model;
using FuriaChatBotApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FuriaChatBotApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase {
        private readonly IChatService _chatService;
        private readonly ICacheService _cacheService;

        private readonly string _mensagemInicial = 
            $"Olá! Eu sou o ChatBot que te fala informações fresquinhas sobre a FURIA 🐺!\n\n" +
            $"Me diz aí, o que você quer saber sobre a FURIA hoje?\n" +
            $"Posso te falar os atuais times da FURIA ou quais serão as próximas partidas, é só perguntar!";

        private readonly List<string> _opcoesIniciais = new List<string>() {
            "Qual o atual time de CS-GO da FURIA?",
            "Qual o atual time de Valorant da FURIA?",
            "Qual o próximo jogo da FURIA?",
            "Qual o resultado do último jogo da FURIA no R6?"
        };

        public ChatController(IChatService chatService, ICacheService memoryCache) {
            _chatService = chatService;
            _cacheService = memoryCache;
        }

        /// <summary>
        /// Rota para obtenção de um Id de sessão da API
        /// </summary>
        /// <returns>
        /// Modelo de resposta com a mensagem inicial padrão e o id da sessão criada
        /// </returns>
        [HttpGet("getSession")]
        public async Task<ActionResult<ChatResponse>> GetSession() {
            var sessionId = Guid.NewGuid().ToString();

            // Cria um contexto inicial
            var initialContext = new SessionContext {
                SessionId = sessionId,
                CurrentStep = "Start",
                LastInteraction = DateTime.UtcNow
            };

            await _cacheService.SaveContextAsync(sessionId, initialContext);

            return Ok(new ChatResponse(sessionId, _mensagemInicial, _opcoesIniciais, ChatResponse.CodErro.Ok));
        }

        /// <summary>
        /// Rota de obtenção da resposta do ChatBot através da requisição do usuário
        /// </summary>
        /// <param name="request">Requisição com id da sessão e pergunta do usuário</param>
        /// <returns>
        /// Modelo de resposta com base na requisição
        /// </returns>
        [HttpPost("ask")]
        public async Task<ActionResult<ChatResponse>> Ask([FromBody] ChatRequest request) {
            bool isValid = await _cacheService.IsSessionValidAsync(request.SessionId);

            ChatResponse reply;
            if (isValid) {
                reply = await _chatService.GetResponseAsync(request.SessionId, request.Message);
                return Ok(reply);
            } else {
                return await GetSession();
            }
        }
    }
}
