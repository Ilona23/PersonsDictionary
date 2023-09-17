using MediatR;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Models;

namespace Application.Persons.Commands.CreatePerson
{
    public sealed record CreatePersonCommand(
        int Id,
        string FirstName,
        string LastName,
        string PersonalId,
        DateTime BirthDate,
        int CityId,
        Gender Gender,
        IEnumerable<PhoneNumberModel> PhoneNumbers) : ICommand<Unit>;
}
