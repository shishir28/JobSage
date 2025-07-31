using JobSage.Domain.Entities;
using JobSage.Domain.Enums;
using MediatR;

namespace JobSage.Application.Jobs.Commands
{
    public class CreateJobCommand : IRequest<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public JobType JobType { get; set; }
        public JobPriority Priority { get; set; }
        public JobStatus Status { get; set; }
        public PropertyInformation PropertyInfo { get; set; } = new();
        public SchedulingInfo Scheduling { get; set; } = new();
        public JobCost Cost { get; set; } = new();
        public Contractor Contractor { get; set; } = new();
        public Guid CreatedBy { get; set; }
        public Guid? AssignedTo { get; set; }
        public string? TenantContact { get; set; }
    }

    public class UpdateJobCommand : IRequest
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
        public Contractor Contractor { get; set; } = new();
        public Guid CreatedBy { get; set; }
        public Guid? AssignedTo { get; set; }
        public string? TenantContact { get; set; }
    }

    public class DeleteJobCommand : IRequest
    {
        public Guid Id { get; set; }
    }

}
