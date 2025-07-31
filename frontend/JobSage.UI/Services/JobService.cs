using JobSage.UI.Models;

namespace JobSage.UI.Services
{
    public interface IJobService
    {
        Task<List<JobDto>> GetJobs();
        Task<JobDto> GetJobById(Guid id);
        Task AddJob(JobDto job);
        Task UpdateJob(JobDto job);
        Task DeleteJob(Guid id);
        Task<List<ContractorDto>> GetAllContractorsAsync();
    }

    public class JobService : IJobService
    {
        private readonly HttpClient _httpClient;

        public JobService(HttpClient httpClient)
        {
            _httpClient = httpClient!;
        }

        public async Task<List<JobDto>> GetJobs() =>
            await _httpClient.GetFromJsonAsync<List<JobDto>>("api/jobs") ?? [];


        public async Task<JobDto> GetJobById(Guid id)
        {
            var job = await _httpClient.GetFromJsonAsync<JobDto>($"api/jobs/{id}");
            if (job == null)
                throw new InvalidOperationException($"Job with id {id} was not found.");
            return job;
        }

        public async Task AddJob(JobDto job) => await _httpClient.PostAsJsonAsync("api/jobs", job);

        public async Task UpdateJob(JobDto job) =>
            await _httpClient.PutAsJsonAsync($"api/jobs/{job.Id}", job);

        public async Task DeleteJob(Guid id) => await _httpClient.DeleteAsync($"api/jobs/{id}");

        public async Task<List<ContractorDto>> GetAllContractorsAsync()
        {
            var contractors = await _httpClient.GetFromJsonAsync<List<ContractorDto>>("api/contractors");
            return contractors ?? new List<ContractorDto>();
        }

    }
}