using JobSage.Domain.Entities;
using JobSage.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
// Ensure the interface is defined and the correct using is present

namespace JobSage.Infrastructure.Persistence.Repositories
{
    public class ContractorRepository : IContractorRepository
    {
        private readonly JobSageDbContext _context;

        public ContractorRepository(JobSageDbContext context)
        {
            _context = context;
        }

        public async Task<Contractor?> GetByIdAsync(string id) =>
            await _context.Contractors.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<List<Contractor>> GetAllAsync() =>
            await _context.Contractors.ToListAsync();

        public async Task AddAsync(Contractor contractor)
        {
            await _context.Contractors.AddAsync(contractor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contractor contractor)
        {
            _context.Contractors.Update(contractor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var contractor = await _context.Contractors.FirstOrDefaultAsync(c => c.Id == id);
            if (contractor != null)
            {
                _context.Contractors.Remove(contractor);
                await _context.SaveChangesAsync();
            }
        }
    }
}