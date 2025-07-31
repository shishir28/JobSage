using JobSage.Domain.Entities;
using JobSage.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobSage.Infrastructure.Persistence.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly JobSageDbContext _context;

        public JobRepository(JobSageDbContext context) => _context = context;

        public async Task<Job?> GetByIdAsync(Guid Id) =>
            await _context.Jobs
                .Include(j => j.Contractor)
                .Include(j => j.PropertyInfo)
                .Include(j => j.Scheduling)
                .Include(j => j.Cost)
                .FirstOrDefaultAsync(j => j.Id == Id);

        public async Task<List<Job>> GetAllAsync() =>
            await _context.Jobs
                .Include(j => j.Contractor)
                .Include(j => j.PropertyInfo)
                .Include(j => j.Scheduling)
                .Include(j => j.Cost)
                .ToListAsync();
        public async Task<Guid> AddAsync(Job job)
        {
            job.Contractor = null; // Ensure only ContractorId is set, and Contractor is not tracked
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

        public void EnsureUtcDates(Job job)
        {
            if (job.Scheduling.DueDate.HasValue)
                job.Scheduling.DueDate = DateTime.SpecifyKind(job.Scheduling.DueDate.Value, DateTimeKind.Utc);

            if (job.Scheduling.ScheduledDate != default)
                job.Scheduling.ScheduledDate = DateTime.SpecifyKind(job.Scheduling.ScheduledDate, DateTimeKind.Utc);
            if (job.Scheduling.CompletedAt.HasValue)
                job.Scheduling.CompletedAt = DateTime.SpecifyKind(job.Scheduling.CompletedAt.Value, DateTimeKind.Utc);
        }
    }
}