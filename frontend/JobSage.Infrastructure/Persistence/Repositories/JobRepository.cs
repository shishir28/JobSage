using JobSage.Domain.Entities;
using JobSage.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobSage.Infrastructure.Persistence.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly JobSageDbContext _context;

        public JobRepository(JobSageDbContext context) =>
            _context = context;

        public async Task<Job?> GetByIdAsync(Guid Id) =>
             await _context.Jobs.FindAsync(Id);

        public async Task<List<Job>> GetAllAsync() =>
             await _context.Jobs.ToListAsync();

        public async Task<Guid> AddAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();
            return job.Id;
        }

        public async Task UpdateAsync(Job job)
        {
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(Guid Id)
        {
            var job = await _context.Jobs.FindAsync(Id);
            _context.Jobs.Remove(job!);
            await _context.SaveChangesAsync();
        }

    }
}


