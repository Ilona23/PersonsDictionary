using Application.Persons.Commands.CreatePerson;
using Domain.Entities;

namespace Application.Abstractions.Messaging
{
    public interface IDtoToEntityMapper
    {
        Person MapToEntity(CreatePersonCommand command);
    }
}
