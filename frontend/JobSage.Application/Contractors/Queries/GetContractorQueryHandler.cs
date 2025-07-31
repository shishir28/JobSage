using JobSage.Domain.Repositories;
using MediatR;

namespace JobSage.Application.Contractors.Queries
{
    public class GetContractorByIdQueryHandler
        : IRequestHandler<GetContractorByIdQuery, GetContractorByIdQueryResult?>
    {
        private readonly IContractorRepository _repository;

        public GetContractorByIdQueryHandler(IContractorRepository repository) =>
            _repository = repository;

        public async Task<GetContractorByIdQueryResult?> Handle(
            GetContractorByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var contractor = await _repository.GetByIdAsync(request.Id);
            if (contractor == null)
                return null;

            return new GetContractorByIdQueryResult(
                contractor.Id,
                contractor.Name,
                contractor.Trade,
                contractor.Rating,
                contractor.Availability,
                contractor.ContactInfo,
                contractor.Location,
                contractor.HourlyRate,
                contractor.Preferred,
                contractor.WarrantyApproved
            );
        }
    }

    public class GetAllContractorsQueryHandler
        : IRequestHandler<GetAllContractorsQuery, List<GetAllContractorsQueryResult>>
    {
        private readonly IContractorRepository _repository;

        public GetAllContractorsQueryHandler(IContractorRepository repository) =>
            _repository = repository;

        public async Task<List<GetAllContractorsQueryResult>> Handle(
            GetAllContractorsQuery request,
            CancellationToken cancellationToken
        )
        {
            var contractors = await _repository.GetAllAsync();
            return contractors
                .Select(contractor => new GetAllContractorsQueryResult(
                    contractor.Id,
                    contractor.Name,
                    contractor.Trade,
                    contractor.Rating,
                    contractor.Availability,
                    contractor.ContactInfo,
                    contractor.Location,
                    contractor.HourlyRate,
                    contractor.Preferred,
                    contractor.WarrantyApproved
                ))
                .ToList();
        }
    }
}
