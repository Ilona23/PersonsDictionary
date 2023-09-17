using MediatR;
using Domain.Abstractions;
using Application.Persons.Queries.GetPersons;
using Domain.Models;

namespace Application.Persons.Queries.GetPersonById
{
    public class GetPersonsRelationsHandler : IRequestHandler<GetPersonsRelationsQuery, IQueryable<PersonsRelationsModel>>
    {
        private readonly IPersonRepository _repository;

        public GetPersonsRelationsHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<IQueryable<PersonsRelationsModel>> Handle(GetPersonsRelationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetPersonsRelationsAsync(cancellationToken);

            return result;
        }
    }
}
