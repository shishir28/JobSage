using System;
using System.Collections.Generic;

namespace JobSage.Domain.Entities
{
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
        
        // Navigation property
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }

    public class PropertyInformation
    {
        public Guid PropertyId { get; set; }
        public string PropertyAddress { get; set; } = string.Empty;
        public string UnitNumber { get; set; } = string.Empty;
        public string LocationDetails { get; set; } = string.Empty;
    }

    public class SchedulingInfo
    {
        public Guid SchedulingId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
