using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using JobSage.UI.Models;

namespace JobSage.UI.Services
{
    public interface IChatService
    {
        Task<JsonElement> SendMessageAsync(JobDto job, string agentKind);
        Task<Dictionary<string, AgentDto>> GetAgentsAsync();
        Task<ChatMessageResponse> SendChatMessageAsync(string message, string? conversationId = null, Dictionary<string, object>? filter = null, string? nameSpace = null);
    }

    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<JsonElement> SendMessageAsync(JobDto job, string agentKind)
        {
            var request = new
            {
                AgentKind = agentKind,
                job.Title,
                job.Description,
            };

            var response = await _httpClient.PostAsJsonAsync($"api/chat/{job.Id}", request);
            var jsonContent = await response.Content.ReadFromJsonAsync<JsonElement>();

            if (jsonContent.TryGetProperty("reply", out var summary))
            {
                if (summary.ValueKind == JsonValueKind.String)
                {
                    var summaryString = summary.GetString();
                    if (!string.IsNullOrEmpty(summaryString))
                    {
                        try
                        {
                            return StringAsJsonElement(summaryString);
                        }
                        catch (JsonException)
                        {
                            return EmptyJsonElement();
                        }
                    }
                }
                return summary;
            }
            return EmptyJsonElement();
        }

        public async Task<Dictionary<string, AgentDto>> GetAgentsAsync()
        {
            var response = await _httpClient.GetAsync("api/chat/agents");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Failed to fetch agents: {response.StatusCode}");

            var agentsJson = await response.Content.ReadAsStringAsync();
            var agents = JsonSerializer.Deserialize<Dictionary<string, AgentDto>>(agentsJson);

            if (agents == null)
                throw new InvalidOperationException("Failed to deserialize agents response.");

            return agents;
        }

        private static JsonElement EmptyJsonElement() => StringAsJsonElement("{}");

        private static JsonElement StringAsJsonElement(string value)
        {
            using var parsedSummaryDocument = JsonDocument.Parse(value);
            return parsedSummaryDocument.RootElement.Clone();
        }

        public async Task<ChatMessageResponse> SendChatMessageAsync(
            string message,
            string? conversationId = null,
            Dictionary<string, object>? filter = null,
            string? nameSpace = null)
        {
            var request = new ChatMessageRequest
            {
                Message = message,
                ConversationId = conversationId,
                Filter = filter,
                Namespace = nameSpace
            };

            var response = await _httpClient.PostAsJsonAsync("api/chat/messages", request);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Chat request failed: {response.StatusCode}");

            var chatResponse = await response.Content.ReadFromJsonAsync<ChatMessageResponse>();

            if (chatResponse == null)
                throw new InvalidOperationException("Failed to deserialize chat response.");

            return chatResponse;
        }
    }
}

namespace JobSage.UI.Models
{
    public class AgentDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
