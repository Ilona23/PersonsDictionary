using Application.Persons.Queries.GetPersons;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ISender _sender;

        public ReportsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IQueryable<PersonsRelationsModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RelatedPersons([FromQuery] GetPersonsRelationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(request, cancellationToken);
            return Ok(result);
        }
    }
}
