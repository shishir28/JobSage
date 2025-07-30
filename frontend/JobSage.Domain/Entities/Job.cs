using System;
using JobSage.Domain.Enums;

namespace JobSage.Domain.Entities
{
    public class Job
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public JobType JobType { get; set; }
        public JobPriority Priority { get; set; }
        public JobStatus Status { get; set; }

        // Foreign Key for PropertyInformation
        public Guid PropertyInfoId { get; set; }
        public PropertyInformation PropertyInfo { get; set; } = null!;

        // Foreign Key for SchedulingInfo
        public Guid SchedulingId { get; set; }
        public SchedulingInfo Scheduling { get; set; } = null!;

        public JobCost Cost { get; set; } = new();
        public Guid CreatedBy { get; set; }
        public Guid? AssignedTo { get; set; }
        public string? TenantContact { get; set; }

        // Foreign Key for Contractor
        public string? ContractorId { get; set; }
        public Contractor? Contractor { get; set; }
    }

    public class PropertyInformation
    {
        public Guid PropertyId { get; set; }
        public string PropertyAddress { get; set; } = string.Empty;
        public string? UnitNumber { get; set; }
        public string? LocationDetails { get; set; }
    }

    public class SchedulingInfo
    {
        public Guid SchedulingId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class JobCost
    {
        public decimal? EstimatedCost { get; set; }
        public decimal? ActualCost { get; set; }
        public decimal? ApprovedBudget { get; set; }
    }

    public class Contractor
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Trade { get; set; } = string.Empty;
        public float Rating { get; set; }
        public string Availability { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public float HourlyRate { get; set; }
        public bool Preferred { get; set; }
        public bool WarrantyApproved { get; set; }
    }
}
