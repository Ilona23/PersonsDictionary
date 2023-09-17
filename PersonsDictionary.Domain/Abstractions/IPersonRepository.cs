using Domain.Entities;
using Domain.Models;

namespace Domain.Abstractions
{
    public interface IPersonRepository
    {
        Task<Person> GetPersonByIdAsync(int id, CancellationToken cancellationToken);

        Task<Person> GetPersonByIdDetailedAsync(int id, CancellationToken cancellationToken);

        Task<Person> GetPersonByPersonalIdAsync(string personalId, CancellationToken cancellationToken);

        Task<List<Person>> GetPersonsAsync(CancellationToken cancellationToken);

        Task InsertAsync(Person entity, CancellationToken cancellationToken);

        Task UpdateAsync(Person entity);

        void Delete(Person entity);

        Task<IQueryable<PersonsRelationsModel>> GetPersonsRelationsAsync(CancellationToken cancellationToken);
    }
}
