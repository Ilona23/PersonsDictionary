using Domain.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Persons.Queries.GetPersons
{
    public class GetPersonsSearchQueryHandler : IRequestHandler<GetPersonsSearchQuery, PagedResult<Person>>
    {
        private readonly IPersonRepository _repository;

        public GetPersonsSearchQueryHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<Person>> Handle(GetPersonsSearchQuery searchCriteria, CancellationToken cancellationToken)
        {
            var result = await _repository.SearchPersonsAsync(
                searchCriteria.QuickSearch,
                searchCriteria.FirstName,
                searchCriteria.LastName,
                searchCriteria.PersonalId,
                searchCriteria.Page,
                searchCriteria.PageSize,
                cancellationToken);

            return result;
        }
    }
}
