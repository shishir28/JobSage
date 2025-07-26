using JobSage.Domain.Entities;
using JobSage.Domain.Repositories;
using Mapster;
using MediatR;

namespace JobSage.Application.Jobs.Queries
{
    public class GetJobsQuery : IRequest<IEnumerable<Job>> { }

    public class GetJobsQueryHandler : IRequestHandler<GetJobByIdQuery, GetJobByIdQueryResult>
                                      , IRequestHandler<GetAllJobsQuery, List<GetAllJobsQueryResult>>
    {
        private readonly IJobRepository _jobRepository;
        public GetJobsQueryHandler(IJobRepository jobRepository) =>
            _jobRepository = jobRepository;

        public async Task<GetJobByIdQueryResult> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.Id);
            return job.Adapt<GetJobByIdQueryResult>();
        }
        public async Task<List<GetAllJobsQueryResult>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = await _jobRepository.GetAllAsync();
            return jobs.Adapt<List<GetAllJobsQueryResult>>();
        }

    }
}
