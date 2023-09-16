using MediatR;
using Application.Abstractions.Messaging;
using Domain.Models;

namespace Application.Persons.Commands.UpdatePerson;

//public class UpdatePersonCommand : ICommand<Unit>
//{
//    [JsonIgnore]
//    public int Id { get; set; }

//    public string FirstName { get; set; }

//    public string LastName { get; set; }

//    public int CityId { get; set; }

//    public IEnumerable<UpdatePhoneNumberModel> PhoneNumbers { get; set; }
//}


public sealed record UpdatePersonCommand(
    int Id,
    string FirstName,
    string LastName,
    int CityId,
    IEnumerable<UpdatePhoneNumberModel> PhoneNumbers) : ICommand<Unit>;

