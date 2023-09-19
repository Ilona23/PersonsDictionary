using MediatR;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Abstractions;
using Application.Constants;
using Application.Services;
using Application.Abstractions.Messaging;
using Application.Models;

namespace Application.Persons.Commands.CreatePersonRelation
{
    public class CreatePersonRelationHandler : IRequestHandler<CreatePersonRelationCommand, RelatedPersonsModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _repository;
        private readonly IResourceManagerService _resourceManagerService;
        private readonly IMapper<PersonRelation, RelatedPersonsModel> _relatedPersonsMapper;

        public CreatePersonRelationHandler(IPersonRepository repository,
            IUnitOfWork unitOfWork,
            IResourceManagerService resourceManagerService,
            IMapper<PersonRelation, RelatedPersonsModel> relatedPersonsMapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _resourceManagerService = resourceManagerService;
            _relatedPersonsMapper = relatedPersonsMapper;
        }

        public async Task<RelatedPersonsModel> Handle(CreatePersonRelationCommand request, CancellationToken cancellationToken)
        {
            var person = await _repository.GetPersonByIdDetailedAsync(request.PersonId, cancellationToken);
            if (person is null)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
                throw new NotFoundException(string.Format(message, request.PersonId), true);
            }

            var relatedPerson = await _repository.GetPersonByIdDetailedAsync(request.RelatedPersonId, cancellationToken);
            if (relatedPerson is null)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.RelatedPersonNotFoundById);
                throw new NotFoundException(string.Format(message, request.RelatedPersonId), true);
            }

            var personRelation = new PersonRelation(person, relatedPerson, request.RelatedType);

            person.RelatedPersons.Add(personRelation);
            relatedPerson.RelatedToPersons.Add(personRelation);

            _repository.Update(personRelation);

            await _unitOfWork.CommitAsync(cancellationToken);

            return _relatedPersonsMapper.MapToModel(personRelation);
        }
    }
}
