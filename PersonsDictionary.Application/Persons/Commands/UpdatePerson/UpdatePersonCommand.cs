using Application.Abstractions.Messaging;
using Application.Models;
using Domain.Enums;

namespace Application.Persons.Commands.UpdatePerson
{
    public sealed record UpdatePersonCommand(
        int Id,
        string FirstName,
        string LastName,
        string PersonalId,
        DateTime BirthDate,
        int CityId,
        Gender Gender,
        IEnumerable<UpdatePhoneNumberModel> PhoneNumbers) : ICommand<PersonModel>;
}
