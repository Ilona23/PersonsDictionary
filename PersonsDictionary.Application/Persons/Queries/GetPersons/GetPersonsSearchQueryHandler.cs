using Application.Abstractions.Messaging;
using Application.Models;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Persons.Queries.GetPersons
{
    public class GetPersonsSearchQueryHandler : IRequestHandler<GetPersonsSearchQuery, PagedResult<PersonModel>>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper<Person, PersonModel> _personMapper;

        public GetPersonsSearchQueryHandler(IPersonRepository repository, IMapper<Person, PersonModel> personMapper)
        {
            _repository = repository;
            _personMapper = personMapper;
        }

        public async Task<PagedResult<PersonModel>> Handle(GetPersonsSearchQuery searchCriteria, CancellationToken cancellationToken)
        {
            var sourcePagedResult = await _repository.SearchPersonsAsync(
                searchCriteria.QuickSearch,
                searchCriteria.FirstName,
                searchCriteria.LastName,
                searchCriteria.PersonalId,
                searchCriteria.Page,
                searchCriteria.PageSize,
                cancellationToken);

            var pagedResultPersonModel = new PagedResult<PersonModel>
            {
                TotalCount = sourcePagedResult.TotalCount,
                Page = sourcePagedResult.Page,
                PageSize = sourcePagedResult.PageSize,
                Results = _personMapper.MapToModelList(sourcePagedResult.Results),
            };

            return pagedResultPersonModel;
        }
    }
}
