using JobSage.Application.Contractors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JobSage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContractorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all contractors")]
        [SwaggerResponse(200, "List of contractors", typeof(List<GetAllContractorsQuery>))]
        public async Task<IActionResult> GetAllContractors()
        {
            var contractors = await _mediator.Send(new GetAllContractorsQuery());
            return Ok(contractors);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a contractor by ID")]
        [SwaggerResponse(200, "Contractor found", typeof(GetContractorByIdQueryResult))]
        [SwaggerResponse(404, "Contractor not found")]
        public async Task<IActionResult> GetContractorById(string id)
        {
            var contractor = await _mediator.Send(new GetContractorByIdQuery(id));
            if (contractor == null)
                return NotFound();
            return Ok(contractor);
        }
    }
}