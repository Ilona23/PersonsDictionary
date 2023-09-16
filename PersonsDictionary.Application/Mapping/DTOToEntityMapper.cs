using Application.Persons.Commands.CreatePerson;
using Domain.Abstractions;
using Domain.Entities;

namespace Domain.Mapping
{
    public class DTOToEntityMapper : IDTOToEntityMapper
    {
        public Person ConvertDTOToEntity(CreatePersonCommand command)
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
