using JobSage.Application.Jobs.Commands;
using JobSage.Application.Jobs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JobSage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new job")]
        [SwaggerResponse(201, "Job created successfully", typeof(Guid))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobCommand command)
        {
            var jobId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetJobById), new { id = jobId }, jobId);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates an existing jon")]
        [SwaggerResponse(204, "Job updated successfully")]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdateJobCommand command)
        {
            if (id != command.Id)
                return BadRequest("joB ID mismatch");

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a job by Id")]
        [SwaggerResponse(200, "Job retrieved successfully", typeof(GetJobByIdQueryResult))]
        [SwaggerResponse(404, "Job not found")]
        public async Task<IActionResult> GetJobById(Guid id)
        {
            var query = new GetJobByIdQuery(id);
            var job = await _mediator.Send(query);

            if (job == null)
                return NotFound();

            return Ok(job);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all jobs")]
        [SwaggerResponse(200, "Jobs retrieved successfully", typeof(List<GetAllJobsQueryResult>))]
        public async Task<IActionResult> GetAllJobs()
        {
            var query = new GetAllJobsQuery();
            var jobs = await _mediator.Send(query);
            return Ok(jobs.Take(10));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a Job by Id")]
        [SwaggerResponse(204, "Job deleted successfully")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var command = new DeleteJobCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
