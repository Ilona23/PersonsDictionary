using MediatR;

namespace Application.Persons.Commands.DeleteRelatedPerson
{
    public class DeleteRelatedPersonCommand : IRequest<Unit>
    {
        public int PersonId { get; set; }

        public int RelatedPersonId { get; set; }
    }
}
