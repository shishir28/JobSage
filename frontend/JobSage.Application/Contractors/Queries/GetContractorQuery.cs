using JobSage.Domain.Entities;
using MediatR;

namespace JobSage.Application.Contractors.Queries
{
    public class GetContractorByIdQuery : IRequest<GetContractorByIdQueryResult>
    {
        public string Id { get; set; } = string.Empty;
        public GetContractorByIdQuery(string id) => Id = id;
    }

    public class GetContractorByIdQueryResult
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

        public GetContractorByIdQueryResult() { }

        public GetContractorByIdQueryResult(
            string id,
            string name,
            string trade,
            float rating,
            string availability,
            string contactInfo,
            string location,
            float hourlyRate,
            bool preferred,
            bool warrantyApproved)
        {
            Id = id;
            Name = name;
            Trade = trade;
            Rating = rating;
            Availability = availability;
            ContactInfo = contactInfo;
            Location = location;
            HourlyRate = hourlyRate;
            Preferred = preferred;
            WarrantyApproved = warrantyApproved;
        }
    }

    public class GetAllContractorsQuery : IRequest<List<GetAllContractorsQueryResult>>
    {
    }

    public class GetAllContractorsQueryResult
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

        public GetAllContractorsQueryResult() { }

        public GetAllContractorsQueryResult(
            string id,
            string name,
            string trade,
            float rating,
            string availability,
            string contactInfo,
            string location,
            float hourlyRate,
            bool preferred,
            bool warrantyApproved)
        {
            Id = id;
            Name = name;
            Trade = trade;
            Rating = rating;
            Availability = availability;
            ContactInfo = contactInfo;
            Location = location;
            HourlyRate = hourlyRate;
            Preferred = preferred;
            WarrantyApproved = warrantyApproved;
        }
    }
}