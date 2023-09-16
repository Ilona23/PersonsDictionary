using Domain.Entities;
using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Abstractions;

public interface IPersonRepository
{
    Task<Person> GetAsync(int id);
    Task<Person> GetPersonByIdAsync(int id);

    void Delete(Person entity);

    Task InsertAsync(Person entity);

    Task UpdateAsync(Person entity);

    Task<IDictionary<int, Person>> ToDictionaryAsync();

    Task<Person> FirstOrDefaultAsync(Expression<Func<Person, bool>>? expression = null);

    Task<IQueryable<PersonsRelationsModel>> GetPersonsRelationsAsync();

    Task<IQueryable<Person>> QueryAsync(Expression<Func<Person, bool>>? expression = null);
}
