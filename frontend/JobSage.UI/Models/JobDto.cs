using JobSage.Domain.Entities;
using JobSage.Domain.Enums;

namespace JobSage.UI.Models
{
    public class JobDto
    {
        public Guid Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public JobType JobType { get; set; }
        public JobPriority Priority { get; set; }
        public JobStatus Status { get; set; }
        public PropertyInformation? PropertyInfo { get; set; }
        public SchedulingInfo? Scheduling { get; set; }
        public JobCost? Cost { get; set; }
        public Contractor? Contractor { get; set; }
        public void EnsureNonNullNestedObjects()
        {
            PropertyInfo ??= new PropertyInformation();
            Scheduling ??= new SchedulingInfo();
            Cost ??= new JobCost();
            Contractor ??= new Contractor();
        }

        public Guid CreatedBy { get; set; }
        public Guid? AssignedTo { get; set; }
        public string? TenantContact { get; set; }
    }
}
