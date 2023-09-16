using Domain.Entities;
using Domain.Enums;
using Persistence.Data;

namespace Persistance
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            //if (context.Persons.Any())
            //{
            //    return;
            //}

            var persons = new List<Person>
            {
                new Person
                {
                    FirstName = "Ilona",
                    LastName = "Ashkhatoeva",
                    PersonalId = "01001074738",
                    BirthDate = new DateTime(1991, 11, 23),
                    CityId = 1,
                    Gender = Gender.Female,
                },
                new Person
                {
                    FirstName = "Alex",
                    LastName = "Ashkhatoeva",
                    PersonalId = "01001074739",
                    BirthDate = new DateTime(1991, 11, 23),
                    CityId = 1,
                    Gender = Gender.Male,
                },
                new Person
                {
                    FirstName = "Liza",
                    LastName = "Bars",
                    PersonalId = "01001074737",
                    BirthDate = new DateTime(1991, 11, 23),
                    CityId = 1,
                    Gender = Gender.Female,
                },
            };

            context.Persons.AddRange(persons);
            context.SaveChanges();
        }
    }
}
