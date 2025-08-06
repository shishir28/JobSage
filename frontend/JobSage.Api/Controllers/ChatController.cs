using JobSage.Application.Interfaces;
using JobSage.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JobSage.API.Controllers
{
    public class JobChatRequest
    {
        public string AgentKind { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatAgentService _chatAgentService;

        public ChatController(IChatAgentService chatAgentService)
        {
            _chatAgentService =
                chatAgentService ?? throw new ArgumentNullException(nameof(chatAgentService));
        }

        [HttpPost("{jobId}")]
        [SwaggerOperation(Summary = "Gets a job details")]
        [SwaggerResponse(200, "Data", typeof(string))]
        [SwaggerResponse(404, "Job not found")]
        public async Task<IActionResult> SendJobChatRequest(
            [FromRoute] Guid jobId,
            [FromBody] JobChatRequest request)
        {
            var llmResponse = await _chatAgentService.SendMessageAsync(
                jobId,
                request.AgentKind,
                request.Title,
                request.Description
            );
            return Ok(llmResponse);
        }

        [HttpGet("agents")]
        [SwaggerOperation(Summary = "Fetches available agents from the Python backend")]
        [SwaggerResponse(200, "List of agents", typeof(Dictionary<string, object>))]
        [SwaggerResponse(500, "Error fetching agents from the backend")]
        public async Task<IActionResult> GetAgents()
        {
            var agents = await _chatAgentService.GetAgentsAsync();
            return Ok(agents);
        }

        [HttpPost("messages")]
        [SwaggerOperation(Summary = "Send a message to the chat system")]
        [SwaggerResponse(200, "Chat response", typeof(ChatMessageResponse))]
        [SwaggerResponse(400, "Bad request")]
        [SwaggerResponse(500, "Error processing chat message")]
        public async Task<IActionResult> SendChatMessage([FromBody] Models.ChatMessageRequest request)
        {
            try
            {
                var response = await _chatAgentService.SendChatMessageAsync(
                    message: request.Message,
                    conversationId: request.ConversationId,
                    filter: request.Filter,
                    nameSpace: request.Namespace
                );

                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error communicating with chat service: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
