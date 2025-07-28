using JobSage.UI.Models;

namespace JobSage.UI.Services
{
    public interface IChatService
    {
        Task<string> SendMessageAsync(JobDto job, string agentKind);
    }

    public class ChatService: IChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(HttpClient httpClient) =>
            _httpClient = httpClient;

        public async Task<string> SendMessageAsync(JobDto job, string agentKind)
        {
            var request = new
            {
                AgentKind = "summarization",
                job.Title,
                job.Description
            };

            var response = await _httpClient.PostAsJsonAsync($"api/chat/{job.Id}", request);
            return await response.Content.ReadAsStringAsync();
        }

    }
}
