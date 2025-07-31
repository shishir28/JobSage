using JobSage.Domain.Entities;
using JobSage.Domain.Enums;
using MediatR;

namespace JobSage.Application.Jobs.Queries
{
    public class GetJobByIdQuery : IRequest<GetJobByIdQueryResult>
    {
        public Guid Id { get; set; } = default!;
        public GetJobByIdQuery(Guid id) =>
            Id = id;
    }

    public class GetJobByIdQueryResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public JobType JobType { get; set; }
        public JobPriority Priority { get; set; }
        public JobStatus Status { get; set; }
        public PropertyInformation PropertyInfo { get; set; } = new();
        public SchedulingInfo Scheduling { get; set; } = new();
        public JobCost Cost { get; set; } = new();
        public Guid CreatedBy { get; set; }
        public Guid? AssignedTo { get; set; }
        public string? TenantContact { get; set; }
        public Contractor? Contractor { get; set; }

        public GetJobByIdQueryResult(string title,
           string description,
           JobType jobType,
           JobPriority priority,
           JobStatus status,
           PropertyInformation propertyInfo,
           SchedulingInfo scheduling,
           JobCost cost,
           Contractor? contractor,
           Guid createdBy,
           Guid? assignedTo,
           string? tenantContact)
        {
            Title = title;
            Description = description;
            JobType = jobType;
            Priority = priority;
            Status = status;
            PropertyInfo = propertyInfo;
            Scheduling = scheduling;
            Cost = cost;
            Contractor = contractor;
            CreatedBy = createdBy;
            AssignedTo = assignedTo;
            TenantContact = tenantContact;
        }
    }

    public class GetAllJobsQuery : IRequest<List<GetAllJobsQueryResult>>
    {
    }

    public class GetAllJobsQueryResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public JobType JobType { get; set; }
        public JobPriority Priority { get; set; }
        public JobStatus Status { get; set; }
        public PropertyInformation PropertyInfo { get; set; } = new();
        public SchedulingInfo Scheduling { get; set; } = new();
        public JobCost Cost { get; set; } = new();
        public Contractor? Contractor { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? AssignedTo { get; set; }
        public string? TenantContact { get; set; }

        public GetAllJobsQueryResult(
            Guid id,
            string title,
            string description,
            JobType jobType,
            JobPriority priority,
            JobStatus status,
            PropertyInformation propertyInfo,
            SchedulingInfo scheduling,
            JobCost cost,
            Contractor? contractor,
            Guid createdBy,
            Guid? assignedTo,
            string? tenantContact)
        {
            Id = id;
            Title = title;
            Description = description;
            JobType = jobType;
            Priority = priority;
            Status = status;
            PropertyInfo = propertyInfo;
            Scheduling = scheduling;
            Cost = cost;
            Contractor = contractor;
            CreatedBy = createdBy;
            AssignedTo = assignedTo;
            TenantContact = tenantContact;
        }
    }

}