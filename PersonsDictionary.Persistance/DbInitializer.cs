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
            return new List<Person>
            {
                new Person
                {
                    FirstName = "John",
                    LastName = "Smith",
                    PersonalId = "123456789",
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
                    FirstName = "Alice",
                    LastName = "Johnson",
                    PersonalId = "987654321",
                    BirthDate = new DateTime(1990, 5, 25),
                    CityId = 2,
                    Gender = Gender.Female,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-555-5555" }
                    }
                },
                new Person
                {
                    FirstName = "Michael",
                    LastName = "Brown",
                    PersonalId = "456789012",
                    BirthDate = new DateTime(1982, 8, 8),
                    CityId = 3,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-888-8888" }
                    }
                },
                new Person
                {
                    FirstName = "Emily",
                    LastName = "Davis",
                    PersonalId = "234567890",
                    BirthDate = new DateTime(1995, 3, 20),
                    CityId = 4,
                    Gender = Gender.Female,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-999-9999" }
                    }
                },
                new Person
                {
                    FirstName = "David",
                    LastName = "Wilson",
                    PersonalId = "345678901",
                    BirthDate = new DateTime(1988, 12, 10),
                    CityId = 5,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-777-7777" },
                        new PhoneNumber { Number = "555-222-2222" }
                    }
                }
            };
        }
    }
}
