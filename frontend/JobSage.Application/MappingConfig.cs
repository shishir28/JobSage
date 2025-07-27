using JobSage.Application.Jobs.Commands;
using JobSage.Application.Jobs.Queries;
using JobSage.Domain.Entities;
using Mapster;

namespace JobSage.Application.Mappings
{
    public static class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<CreateJobCommand, Job>
            .NewConfig()
            .ConstructUsing(src => new Job
            {
                Title = src.Title,
                Description = src.Description,
                JobType = src.JobType,
                Status = src.Status,
                Cost = src.Cost,
                CreatedBy = src.CreatedBy,
                AssignedTo = src.AssignedTo,
                TenantContact = src.TenantContact,
                PropertyInfo = src.PropertyInfo,
                Scheduling = src.Scheduling == null ? null : new SchedulingInfo
                {
                    SchedulingId = src.Scheduling.SchedulingId,
                    CreatedAt = DateTime.SpecifyKind(src.Scheduling.CreatedAt, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(src.Scheduling.UpdatedAt, DateTimeKind.Utc),
                    DueDate = src.Scheduling.DueDate.HasValue ? DateTime.SpecifyKind(src.Scheduling.DueDate.Value, DateTimeKind.Utc) : (DateTime?)null,
                    ScheduledDate = DateTime.SpecifyKind(src.Scheduling.ScheduledDate, DateTimeKind.Utc),
                    CompletedAt = src.Scheduling.CompletedAt.HasValue ? DateTime.SpecifyKind(src.Scheduling.CompletedAt.Value, DateTimeKind.Utc) : (DateTime?)null
                }
            });

            TypeAdapterConfig<UpdateJobCommand, Job>
            .NewConfig()
            .ConstructUsing(src => new Job
            {
                Id = src.Id,
                Title = src.Title,
                Description = src.Description,
                JobType = src.JobType,
                Status = src.Status,
                Cost = src.Cost,
                CreatedBy = src.CreatedBy,
                AssignedTo = src.AssignedTo,
                TenantContact = src.TenantContact,
                PropertyInfo = src.PropertyInfo,
                Scheduling = src.Scheduling == null ? null : new SchedulingInfo
                {
                    SchedulingId = src.Scheduling.SchedulingId,
                    CreatedAt = DateTime.SpecifyKind(src.Scheduling.CreatedAt, DateTimeKind.Utc),
                    UpdatedAt = DateTime.SpecifyKind(src.Scheduling.UpdatedAt, DateTimeKind.Utc),
                    DueDate = src.Scheduling.DueDate.HasValue ? DateTime.SpecifyKind(src.Scheduling.DueDate.Value, DateTimeKind.Utc) : (DateTime?)null,
                    ScheduledDate = DateTime.SpecifyKind(src.Scheduling.ScheduledDate, DateTimeKind.Utc),
                    CompletedAt = src.Scheduling.CompletedAt.HasValue ? DateTime.SpecifyKind(src.Scheduling.CompletedAt.Value, DateTimeKind.Utc) : (DateTime?)null
                }
            });

            TypeAdapterConfig<Job, GetJobByIdQueryResult>
           .NewConfig()
              .ConstructUsing(src => new GetJobByIdQueryResult(
                src.Title,
                src.Description,
                src.JobType,
                src.Priority,
                src.Status,
                src.PropertyInfo,
                src.Scheduling,
                src.Cost,
                src.CreatedBy,
                src.AssignedTo,
                src.TenantContact));

            TypeAdapterConfig.GlobalSettings.Compile(); // Compile for performance
        }
    }
}
