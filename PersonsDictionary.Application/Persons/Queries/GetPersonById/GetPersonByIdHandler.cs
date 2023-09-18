using MediatR;
using Domain.Exceptions;
using Application.Constants;
using Domain.Abstractions;
using Application.Services;
using Application.Abstractions.Messaging;
using Domain.Entities;
using Application.Models;

namespace Application.Persons.Queries.GetPersonById
{
    public class GetPersonByIdHandler : IRequestHandler<GetPersonByIdQuery, PersonDetailedModel>
    {
        private readonly IPersonRepository _repository;
        private readonly IResourceManagerService _resourceManagerService;
        private readonly IMapper<Person, PersonDetailedModel> _personMapper;

        public GetPersonByIdHandler(IPersonRepository repository,
            IResourceManagerService resourceManagerService,
            IMapper<Person, PersonDetailedModel> personMapper)
        {
            _repository = repository;
            _resourceManagerService = resourceManagerService;
            _personMapper = personMapper;
        }

        public async Task<PersonDetailedModel> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _repository.GetPersonByIdDetailedAsync(request.Id, cancellationToken);

            if (person is null)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
                throw new NotFoundException(string.Format(message, request.Id), true);
            }

            return _personMapper.MapToModel(person);
        }
    }
}
