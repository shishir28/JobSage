using System.Text.Json.Serialization;

namespace JobSage.Application.Interfaces
{
    public interface IChatAgentService
    {
        Task<ChatResponse> SendMessageAsync(Guid jobId, string agent, string title, string description);
        Task<Dictionary<string, AgentDto>> GetAgentsAsync();
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