namespace JobSage.Domain.DTOs
{
    public class ChatMessageResponse
    {
        public string ConversationId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<SourceDocument> SourceDocuments { get; set; } = new();
        public string GeneratedQuestion { get; set; } = string.Empty;
    }

    public class SourceDocument
    {
        public string Content { get; set; } = string.Empty;
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
