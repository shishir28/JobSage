using JobSage.UI.Models;

namespace JobSage.UI.Services
{
    public interface IContractorService
    {
        Task<List<ContractorDto>> GetContractors();
        Task<ContractorDto> GetContractorById(Guid id);
    }

    public class ContractorService : IContractorService
    {
        private readonly HttpClient _httpClient;

        public ContractorService(HttpClient httpClient) => _httpClient = httpClient!;

        public async Task<List<ContractorDto>> GetContractors() =>
            await _httpClient.GetFromJsonAsync<List<ContractorDto>>("api/contractors") ?? [];


        public async Task<ContractorDto> GetContractorById(Guid id)
        {
            var contractor = await _httpClient.GetFromJsonAsync<ContractorDto>($"api/contractors/{id}");
            if (contractor == null)
                throw new InvalidOperationException($"Job with id {id} was not found.");
            return contractor;
        }
    }
}