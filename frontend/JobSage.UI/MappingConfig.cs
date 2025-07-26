using JobSage.Application.Jobs.Commands;
using JobSage.UI.Models;
using Mapster;

namespace JobSage.UI
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CreateJobCommand, JobDto>
                .NewConfig()
                .ConstructUsing(src => new JobDto
                {
                    Title = src.Title,
                    Description = src.Description,
                    JobType = src.JobType,
                    Priority = src.Priority,
                    Status = src.Status,
                    PropertyInfo = src.PropertyInfo,
                    Scheduling = src.Scheduling,
                    Cost = src.Cost,
                    CreatedBy = src.CreatedBy,
                    AssignedTo = src.AssignedTo,
                    TenantContact = src.TenantContact
                });

            TypeAdapterConfig.GlobalSettings.Compile(); // Compile for performance
        }
    }
}