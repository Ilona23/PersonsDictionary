using Application.Persons.Commands.CreatePerson;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IDTOToEntityMapper
    {
        Person ConvertDTOToEntity(CreatePersonCommand command);
    }
}
