using Application.Abstractions.Messaging;
using Application.Models;
using Domain.Enums;

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
        IEnumerable<PhoneNumberModel> PhoneNumbers) : ICommand<PersonModel>;
}
