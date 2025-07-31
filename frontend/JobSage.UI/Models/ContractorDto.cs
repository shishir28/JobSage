using JobSage.Domain.Entities;
using JobSage.Domain.Enums;

namespace JobSage.UI.Models
{
    public class ContractorDto
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
