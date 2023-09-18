using Application.Abstractions.Messaging;
using Application.Persons.Commands.CreatePerson;
using Domain.Entities;

namespace Application.Abstractions.Mapping
{
    public class DtoToEntityMapper : IDtoToEntityMapper
    {
        public Person MapToEntity(CreatePersonCommand command)
        {
            var person = new Person
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                PersonalId = command.PersonalId,
                BirthDate = command.BirthDate,
                CityId = command.CityId,
                Gender = command.Gender,
                PhoneNumbers = command.PhoneNumbers.Select(phone => new PhoneNumber
                {
                    Number = phone.Number,
                    NumberType = phone.NumberType,
                    PersonId = command.Id
                }).ToList()
            };

            return person;
        }
    }
}
