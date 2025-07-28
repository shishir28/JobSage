namespace JobSage.Application.Interfaces
{
    public interface IChatAgentService
    {
        Task<ChatResponse> SendMessageAsync(Guid jobId, string agent, string message);
    }

    public class ChatResponse
    {
        public string Reply { get; set; } = string.Empty;
    }
}