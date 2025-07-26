using System.Text.Json;
using JobSage.Domain.Entities;
using JobSage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobSage.Infrastructure.Seeders
{
    public class SampleDataSeeder
    {
        private readonly JobSageDbContext _context;
        public SampleDataSeeder(JobSageDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync(string propertiesPath, string jobsPath, string schedulingPath)
        {
            var properties = JsonSerializer.Deserialize<List<PropertyInformation>>(await File.ReadAllTextAsync(propertiesPath));
            var jobs = JsonSerializer.Deserialize<List<Job>>(await File.ReadAllTextAsync(jobsPath));
            var schedulingInfos = JsonSerializer.Deserialize<List<SchedulingInfo>>(await File.ReadAllTextAsync(schedulingPath));

            if (properties != null && !_context.Properties.Any())
            {
                _context.Properties.AddRange(properties);
            }
            if (jobs != null && !_context.Jobs.Any())
            {
                _context.Jobs.AddRange(jobs);
            }
            if (schedulingInfos != null && !_context.SchedulingInfos.Any())
            {
                _context.SchedulingInfos.AddRange(schedulingInfos);
            }
            await _context.SaveChangesAsync();
        }
    }
}
