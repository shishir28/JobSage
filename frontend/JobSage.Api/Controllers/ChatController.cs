using JobSage.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
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
            _chatAgentService = chatAgentService ?? throw new ArgumentNullException(nameof(chatAgentService));
        }


        [HttpPost("{jobId}")]
        [SwaggerOperation(Summary = "Gets a job details")]
        [SwaggerResponse(200, "Data", typeof(string))]
        [SwaggerResponse(404, "Job not found")]
        public async Task<IActionResult> SendJobChatRequest([FromRoute] Guid jobId, [FromBody] JobChatRequest request)
        {
            var messageBody = new Dictionary<string, string> { { "tite", request.Title },
             { "description", request.Description }};

            var llmResponse = await _chatAgentService.SendMessageAsync(jobId, request.AgentKind, JsonConvert.SerializeObject(messageBody));
            return Ok(llmResponse);
        }
    }
}
