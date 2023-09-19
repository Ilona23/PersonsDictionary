using Domain.Entities;
using Domain.Models;

namespace Domain.Abstractions
{
    public interface IPersonRepository
    {
        Task<Person?> GetPersonByIdAsync(int id, CancellationToken cancellationToken);

        Task<Person?> GetPersonByIdDetailedAsync(int id, CancellationToken cancellationToken);

        Task<Person?> GetPersonByPersonalIdAsync(string personalId, CancellationToken cancellationToken);

        Task<List<Person>> GetPersonsAsync(CancellationToken cancellationToken);

        Task InsertAsync(Person entity, CancellationToken cancellationToken);

        void Update(Person entity);

        void Update(PersonRelation entity);

        void Delete(Person entity);

        void Delete(PersonRelation entity);

        Task<IQueryable<PersonRelationModel>> GetPersonsRelationsAsync(CancellationToken cancellationToken);

        Task<PagedResult<Person>> SearchPersonsAsync(
            string? QuickSearch,
            string? FirstName,
            string? LastName,
            string? PersonalId,
            int? Page,
            int? PageSize,
            CancellationToken cancellationToken);
    }
}
