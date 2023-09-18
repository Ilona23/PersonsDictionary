using MediatR;

namespace Application.Persons.Commands.DeletePerson
{
    public class DeletePersonCommand : IRequest<DeletePersonResponse>
    {
        public int Id { get; set; }
    }
}
