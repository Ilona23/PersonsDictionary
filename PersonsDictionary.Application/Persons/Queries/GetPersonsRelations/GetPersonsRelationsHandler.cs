using MediatR;
using Domain.Abstractions;
using Domain.Models;

namespace Application.Persons.Queries.GetPersonsRelations
{
    public class GetPersonsRelationsHandler : IRequestHandler<GetPersonsRelationsQuery, IQueryable<PersonRelationModel>>
    {
        private readonly IPersonRepository _repository;

        public GetPersonsRelationsHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<IQueryable<PersonRelationModel>> Handle(GetPersonsRelationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetPersonsRelationsAsync(cancellationToken);

            return result;
        }
    }
}
