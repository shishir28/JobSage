using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobSage.Domain.Entities;

namespace JobSage.Domain.Repositories
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(Guid id);
        Task<List<Job>> GetAllAsync();
        Task<Guid> AddAsync(Job job);
        Task UpdateAsync(Job job);
        Task DeleteByIdAsync(Guid id);
    }
}
