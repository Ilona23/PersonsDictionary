using Domain.Abstractions;
using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PersonRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Person> GetPersonByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Persons.FindAsync(id, cancellationToken);
        }

        public async Task<Person> GetPersonByIdDetailedAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Persons
                .Include(x => x.RelatedToPersons)
                .ThenInclude(x1 => x1.Person)
                .Include(x => x.RelatedPersons)
                .ThenInclude(x2 => x2.RelatedPerson)
                .Include(x => x.PhoneNumbers)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Person> GetPersonByPersonalIdAsync(string personId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Persons.AsNoTracking().FirstOrDefaultAsync(x => x.PersonalId == personId, cancellationToken);
        }

        public async Task<List<Person>> GetPersonsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Persons.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task InsertAsync(Person entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Persons.AddAsync(entity, cancellationToken);
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

        public Task<IQueryable<PersonsRelationsModel>> GetPersonsRelationsAsync(CancellationToken cancellationToken = default)
        {
            var query = _dbContext.PersonRelations
                .AsNoTracking()
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
                .OrderBy(m => m.Key.PersonId)
                .ThenBy(m => m.Key.RelationType)
                .Select(s => new PersonsRelationsModel
                {
                    Id = s.Key.PersonId,
                    FirstName = s.Key.FirstName,
                    LastName = s.Key.LastName,
                    Gender = s.Key.Gender,
                    RelationType = s.Key.RelationType,
                    Count = s.Count
                });

            return Task.FromResult(query);
        }

        public async Task<PagedResult<Person>> SearchPersonsAsync(string QuickSearch,
            string FirstName,
            string LastName,
            string PersonalId,
            int Page,
            int PageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Persons.AsQueryable();

            if (!string.IsNullOrEmpty(QuickSearch))
            {
                query = query.Where(p =>
                    EF.Functions.Like(p.FirstName, $"%{QuickSearch}%") ||
                    EF.Functions.Like(p.LastName, $"%{QuickSearch}%") ||
                    EF.Functions.Like(p.PersonalId, $"%{QuickSearch}%")
                );
            }

            if (!string.IsNullOrEmpty(FirstName))
            {
                query = query.Where(p => p.FirstName.Contains(FirstName));
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                query = query.Where(p => p.LastName.Contains(LastName));
            }

            if (!string.IsNullOrEmpty(PersonalId))
            {
                query = query.Where(p => p.PersonalId.Contains(PersonalId));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            query = query.Skip((Page - 1) * PageSize)
                         .Take(PageSize);

            var results = await query.ToListAsync(cancellationToken);

            return new PagedResult<Person>
            {
                TotalCount = totalCount,
                Page = Page,
                PageSize = PageSize,
                Results = results
            };
        }
    }
}
