using MediatR;
using Application.Abstractions.Messaging;
using Domain.Models;

namespace Application.Persons.Commands.UpdatePerson
{
    public sealed record UpdatePersonCommand(
        int Id,
        string FirstName,
        string LastName,
        int CityId,
        IEnumerable<UpdatePhoneNumberModel> PhoneNumbers) : ICommand<Unit>;
}
