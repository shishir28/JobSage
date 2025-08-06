namespace JobSage.Domain.DTOs
{
    public class ChatMessageRequest
    {
        public string Message { get; set; } = string.Empty;
        public string? ConversationId { get; set; }
        public Dictionary<string, object>? Filter { get; set; }
        public string? Namespace { get; set; }
    }
}
