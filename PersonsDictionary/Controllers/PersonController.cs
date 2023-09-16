using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Persons.Commands.CreatePerson;
using Application.Persons.Commands.CreatePersonRelation;
using Application.Persons.Commands.DeletePerson;
using Application.Persons.Commands.DeleteRelatedPerson;
using Application.Persons.Commands.UpdatePerson;
using Application.Persons.Commands.UpdatePersonImage;
using Application.Persons.Queries.GetPersonById;
using System.ComponentModel.DataAnnotations;

namespace Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ISender _sender;

        public PersonController(ISender sender)
        {
            _sender = sender;
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(List<GetPersonsResponse>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> Persons([FromQuery] GetPersonsQuery request, CancellationToken cancellationToken)
        //{
        //    var result = await _mediator.Send(request, cancellationToken);
        //    return Ok(result);
        //}

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(List<PersonResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedRequestResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPersonById([Required] int id, CancellationToken cancellationToken)
        {
            var query = new GetPersonByIdQuery(id);
            var result = await _sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedRequestResponse), StatusCodes.Status208AlreadyReported)]
        public async Task<IActionResult> Create([FromBody] CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedRequestResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRelationship([FromBody] CreatePersonRelationCommand request, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(request, cancellationToken);
            return Ok(result);
        }

        [Route("{id}")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedRequestResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([Required] int id, [FromBody] UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            request = request with { Id = id };
            Unit result = await _sender.Send(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedRequestResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(FailedRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage([Required] int id, [Required] IFormFile file, CancellationToken cancellationToken)
        {
            UploadPersonImageCommand query = new UploadPersonImageCommand { Id = id, File = file };
            Unit result = await _sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedRequestResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([Required] int id, CancellationToken cancellationToken)
        {
            var query = new DeletePersonCommand { Id = id };
            Unit result = await _sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [Route("{personId}/{relatedPersonId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedRequestResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRelatedPerson([Required] int personId, [Required] int relatedPersonId, CancellationToken cancellationToken)
        {
            var query = new DeleteRelatedPersonCommand { PersonId = personId, RelatedPersonId = relatedPersonId };
            Unit result = await _sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
