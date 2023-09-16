using MediatR;

namespace Application.Persons.Commands.DeletePerson;

public class DeletePersonCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

