using System.Text.Json.Serialization;
using JobSage.Domain.DTOs;

namespace JobSage.Application.Interfaces
{
    public interface IChatAgentService
    {
        Task<ChatResponse> SendMessageAsync(Guid jobId, string agent, string title, string description);
        Task<Dictionary<string, AgentDto>> GetAgentsAsync();
        Task<ChatMessageResponse> SendChatMessageAsync(
            string message,
            string? conversationId = null,
            Dictionary<string, object>? filter = null,
            string? nameSpace = null);
    }

    public class ChatResponse
    {
        public string Reply { get; set; } = string.Empty;
    }


    public class AgentDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}