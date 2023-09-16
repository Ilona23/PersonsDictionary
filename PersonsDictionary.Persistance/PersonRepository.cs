using Domain.Abstractions;
using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence;

public class PersonRepository : IPersonRepository
{
    public ApplicationDbContext _dbContext;
    public PersonRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }


    public async Task<Person> GetAsync(int id)
    {
        return await _dbContext.Persons.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<Person> GetPersonByIdAsync(int id)
    {
        return await _dbContext.Persons
            .Include(x => x.RelatedToPersons)
                .ThenInclude(x1 => x1.Person)
            .Include(x => x.RelatedPersons)
               .ThenInclude(x2 => x2.RelatedPerson)
            .Include(x => x.PhoneNumbers)
            .Where(x => x.Id == id).SingleAsync();
    }

    public async Task InsertAsync(Person entity)
    {
        await _dbContext.Persons.AddAsync(entity);
    }

    public Task UpdateAsync(Person entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbContext.Attach(entity);
        }
        else
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        return Task.CompletedTask;
    }

    public void Delete(Person entity)
    {
        _dbContext.Persons.Remove(entity);
    }

    public async Task<IDictionary<int, Person>> ToDictionaryAsync()
    {
        return await _dbContext.Persons.ToDictionaryAsync(x => x.Id);
    }

    public async Task<Person> FirstOrDefaultAsync(Expression<Func<Person, bool>>? expression = null)
    {
        if (expression is null)
        {
            return await _dbContext.Persons.FirstOrDefaultAsync();
        }

        return await _dbContext.Persons.FirstOrDefaultAsync(expression);
    }

    public async Task<IQueryable<Person>> QueryAsync(Expression<Func<Person, bool>>? expression = null)
    {
        if (expression == null)
        {
            return _dbContext.Persons;
        }

        return _dbContext.Persons.Where(expression);
    }

    public async Task<IQueryable<PersonsRelationsModel>> GetPersonsRelationsAsync()
    {
        var relatedPersons = _dbContext.PersonRelations
            .Include(p => p.Person)
                .GroupBy(x => new
                {
                    x.PersonId,
                    x.RelationType,
                    x.Person.FirstName,
                    x.Person.LastName,
                    x.Person.Gender
                })
                    .Select(c => new
                    {
                        c.Key,
                        Count = c.Count()
                    })
                        .Select(s => new
                        {
                            s.Key.PersonId,
                            s.Key.RelationType,
                            s.Count,
                            s.Key.FirstName,
                            s.Key.LastName,
                            s.Key.Gender
                        })
                            .Select(m => new PersonsRelationsModel
                            {
                                Id = m.PersonId,
                                FirstName = m.FirstName,
                                LastName = m.LastName,
                                Gender = m.Gender,
                                RelationType = m.RelationType,
                                Count = m.Count,
                            });

        return relatedPersons;
    }
}
