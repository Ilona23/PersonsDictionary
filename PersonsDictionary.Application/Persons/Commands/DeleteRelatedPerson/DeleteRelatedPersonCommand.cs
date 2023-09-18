using Application.Persons.Commands.DeletePerson;
using MediatR;

namespace Application.Persons.Commands.DeleteRelatedPerson
{
    public class DeleteRelatedPersonCommand : IRequest<DeleteRelatedPersonResponse>
    {
        public int PersonId { get; set; }
        public int RelatedPersonId { get; set; }
    }
}
