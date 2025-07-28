using JobSage.Application.Interfaces;

namespace JobSage.Infrastructure.Services
{
    public class ChatAgentService : IChatAgentService
    {
        private readonly HttpClient _client;
        public ChatAgentService(HttpClient client) =>
            _client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<ChatResponse> SendMessageAsync(Guid jobId, string agent, string message)
        {
            var jobRequest = new
            {
                JobId = jobId,
                Agent = agent,
                Message = message
            };
            var request = new HttpRequestMessage(HttpMethod.Post, $"/process-job")
            {
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(jobRequest), 
                                            System.Text.Encoding.UTF8, 
                                            "application/json")
            };


            var response = await _client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();
            return new ChatResponse{Reply = responseContent};
        }

        // I want to call process-job, passing the jobId and agent as parameters, and the message as the body of the request.
        // The response should be a ChatResponse object with the reply from the agent.
        
        public async Task<ChatResponse> LoadSummaryAsync(Guid jobId, string message)
        {
            //var request = new HttpRequestMessage(HttpMethod.Post, $"/process-job/{jobId}/{agent}")
            //{
            //    Content = new StringContent(message, System.Text.Encoding.UTF8, "application/json")
            //};
            //var response = await _client.SendAsync(request);
            //if (!response.IsSuccessStatusCode)
            //{
            //    throw new HttpRequestException($"Error sending message: {response.ReasonPhrase}");
            //}
            //var responseContent = await response.Content.ReadAsStringAsync();
            await Task.Delay(1000);
            return new ChatResponse { Reply = "Hello there" };
        }
    }
}
