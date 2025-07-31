using JobSage.Domain.Entities;

namespace JobSage.Domain.Repositories
{
    public interface IContractorRepository
    {
        Task<Contractor?> GetByIdAsync(string id);
        Task<List<Contractor>> GetAllAsync();
        Task AddAsync(Contractor contractor);
        Task UpdateAsync(Contractor contractor);
        Task DeleteAsync(string id);
    }
}