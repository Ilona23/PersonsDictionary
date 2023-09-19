using Application.Models;
using Domain.Entities;

namespace Application.Mapping
{
    public class PersonMapper : BaseMapper<Person, PersonModel>
    {
        public override PersonModel MapToModel(Person entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new PersonModel
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                PersonalId = entity.PersonalId,
                BirthDate = entity.BirthDate,
                CityId = entity.CityId,
                Gender = entity.Gender,
                PhoneNumbers = entity.PhoneNumbers?.Select(MapToPhoneNumberModel).ToList(),
            };
        }

        public override Person MapToEntity(PersonModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new Person
            {
                CreatedDate = DateTime.Now,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PersonalId = model.PersonalId,
                BirthDate = model.BirthDate,
                CityId = model.CityId,
                Gender = model.Gender
            };
        }

        private PhoneNumberModel MapToPhoneNumberModel(PhoneNumber phoneNumber)
        {
            if (phoneNumber == null)
            {
                return null;
            }

            return new PhoneNumberModel
            {
                Number = phoneNumber.Number,
                NumberType = phoneNumber.NumberType
            };
        }

        public override List<PersonModel> MapToModelList(IEnumerable<Person> entities)
        {
            return entities.Select(MapToModel).ToList();
        }

        public override List<Person> MapToEntityList(IEnumerable<PersonModel> models)
        {
            return models.Select(MapToEntity).ToList();
        }
    }
}
