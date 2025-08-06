using System.Text.Json.Serialization;

namespace JobSage.UI.Models
{
    public class ChatMessageRequest
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("conversation_id")]
        public string? ConversationId { get; set; }

        [JsonPropertyName("filter")]
        public Dictionary<string, object>? Filter { get; set; }

        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }
    }
}
