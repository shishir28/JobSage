using System.Net.Http.Json;
using System.Text.Json;
using JobSage.Application.Interfaces;

namespace JobSage.Infrastructure.Services
{
    public class ChatAgentService : IChatAgentService
    {
        private readonly HttpClient _client;

        public ChatAgentService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ChatResponse> SendMessageAsync(
            Guid jobId,
            string agent,
            string title,
            string description
        )
        {
            var jobRequest = new
            {
                JobId = jobId,
                Agent = agent,
                Title = title,
                Description = description,
            };

            var response = await _client.PostAsJsonAsync("jobs/process-job", jobRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            return new ChatResponse { Reply = responseContent };
        }

        public async Task<Dictionary<string, AgentDto>> GetAgentsAsync()
        {
            var response = await _client.GetAsync("jobs/agents");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Failed to fetch agents: {response.StatusCode}");

            var agentsJson = await response.Content.ReadAsStringAsync();
            var agents = JsonSerializer.Deserialize<Dictionary<string, AgentDto>>(agentsJson);

            if (agents == null)
                throw new InvalidOperationException("Failed to deserialize agents response.");

            return agents;
        }
    }
}
