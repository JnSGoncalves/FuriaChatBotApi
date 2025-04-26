using FuriaChatBotApi.Interface;
using FuriaChatBotApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace FuriaChatBotApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService) {
            _chatService = chatService;
        }

        [HttpPost("ask")]
        public async Task<ActionResult<ChatResponse>> Ask([FromBody] ChatRequest request) {
            ChatResponse reply = await _chatService.GetResponseAsync(request.Message);
            return Ok(new ChatResponse($"Resposta de: {reply.Reply}"));
        }
    }
}
