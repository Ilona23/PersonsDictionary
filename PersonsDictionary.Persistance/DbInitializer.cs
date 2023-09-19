using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using System.Text.Json;
using Application.Models;

namespace Persistence
{
    public class DbInitializer
    {
        public async Task Seed(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await _dbContext.Database.MigrateAsync();

            var persons = PersonsData();

            var existingIds = await _dbContext.Persons.Select(p => p.PersonalId).ToListAsync(cancellationToken);
            var personsToAdd = persons.Where(p => !existingIds.Contains(p.PersonalId)).ToList();

            if (personsToAdd.Any())
            {
                await _dbContext.Persons.AddRangeAsync(personsToAdd);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            var citiesList = await _dbContext.Cities.ToListAsync(cancellationToken);

            if (!citiesList.Any())
            {
                using var jsonStream = File.OpenRead("Cities.json");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var result = await JsonSerializer.DeserializeAsync<CityListModel>(jsonStream, options);

                result.Cities.ForEach(x => x.SetCreateDate());

                await _dbContext.Cities.AddRangeAsync(result.Cities, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private static IEnumerable<Person> PersonsData()
        {
            var persons = new List<Person>
            {
                new Person
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    FirstName = "John",
                    LastName = "Smith",
                    PersonalId = "01987654321",
                    BirthDate = new DateTime(1985, 10, 15),
                    CityId = 1,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-123-4567" },
                        new PhoneNumber { Number = "555-987-6543" }
                    }
                },
                new Person
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    PersonalId = "01987654322",
                    BirthDate = new DateTime(1990, 5, 25),
                    CityId = 1,
                    Gender = Gender.Female,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-555-5555" }
                    }
                },
                new Person
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    FirstName = "Michael",
                    LastName = "Brown",
                    PersonalId = "01987354321",
                    BirthDate = new DateTime(1982, 8, 8),
                    CityId = 1,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-888-8888" }
                    }
                },
                new Person
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    FirstName = "Emily",
                    LastName = "Davis",
                    PersonalId = "01987354328",
                    BirthDate = new DateTime(1995, 3, 20),
                    CityId = 1,
                    Gender = Gender.Female,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-999-9999" }
                    }
                },
                new Person
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    FirstName = "David",
                    LastName = "Wilson",
                    PersonalId = "01987314328",
                    BirthDate = new DateTime(1988, 12, 10),
                    CityId = 1,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-777-7777" },
                        new PhoneNumber { Number = "555-222-2222" }
                    }
                 },
                 new Person
                 {
                     CreatedDate = DateTime.Now,
                     UpdatedDate = DateTime.Now,
                     FirstName = "Jacob",
                     LastName = "Blue",
                     PersonalId = "01987328328",
                     BirthDate = new DateTime(1988, 12, 10),
                     CityId = 1,
                     Gender = Gender.Male,
                     PhoneNumbers = new List<PhoneNumber>
                     {
                         new PhoneNumber { Number = "555-888-7777" },
                         new PhoneNumber { Number = "555-222-2222" }
                     }
                 }
            };

            var relations = new List<PersonRelation>
            {
                new PersonRelation(persons[0], persons[1], RelationType.Colleague),
                new PersonRelation(persons[0], persons[2], RelationType.Familiar),
                new PersonRelation(persons[0], persons[3], RelationType.Familiar),
                new PersonRelation(persons[1], persons[3], RelationType.Colleague),
                new PersonRelation(persons[1], persons[4], RelationType.Colleague),
                new PersonRelation(persons[2], persons[4], RelationType.Relative),
                new PersonRelation(persons[3], persons[4], RelationType.Relative),
                new PersonRelation(persons[4], persons[5], RelationType.Other)
            };

            foreach (var relation in relations)
            {
                var person = relation.Person;
                var relatedPerson = relation.RelatedPerson;

                person.RelatedPersons.Add(relation);
                relatedPerson.RelatedToPersons.Add(relation);
            }
            return persons;
        }

    }
}
