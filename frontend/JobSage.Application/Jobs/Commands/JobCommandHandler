using JobSage.Domain.Entities;
using JobSage.Domain.Repositories;
using MediatR;
using Mapster;

namespace JobSage.Application.Jobs.Commands
{
    public class JobCommandHandler : IRequestHandler<CreateJobCommand, Guid>,
                                     IRequestHandler<UpdateJobCommand>,
                                     IRequestHandler<DeleteJobCommand>
    {
        private IJobRepository _jobRepository;

        public JobCommandHandler(IJobRepository jobRepository) =>
            _jobRepository = jobRepository;

        public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            var jobObject = request.Adapt<Job>();
            return await _jobRepository.AddAsync(jobObject);
        }

        public async Task Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var jobObject = request.Adapt<Job>();
            await _jobRepository.UpdateAsync(jobObject);
        }

        public async Task Handle(DeleteJobCommand request, CancellationToken cancellationToken) =>
            await _jobRepository.DeleteByIdAsync(request.Id);
    }
}
