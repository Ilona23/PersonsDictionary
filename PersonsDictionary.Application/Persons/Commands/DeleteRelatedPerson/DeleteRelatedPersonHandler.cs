using MediatR;
using Application.Constants;
using Domain.Exceptions;
using Application.Services;
using Domain.Abstractions;

namespace Application.Persons.Commands.DeleteRelatedPerson
{
    public class DeleteRelatedPersonHandler : IRequestHandler<DeleteRelatedPersonCommand, Unit>
    {
        private readonly IPersonRepository _repository;
        private readonly IResourceManagerService _resourceManagerService;

        public DeleteRelatedPersonHandler(IPersonRepository repository, IResourceManagerService resourceManagerService)
        {
            _repository = repository;
            _resourceManagerService = resourceManagerService;
        }

        public async Task<Unit> Handle(DeleteRelatedPersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _repository.GetAsync(request.PersonId);
            if (person is null)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.DeleteRelatedPersonFailed);
                throw new NotFoundException(message, true);
            }

            var relatedPerson = person.RelatedPersons.FirstOrDefault(x => x.RelatedPersonId == request.RelatedPersonId);
            if (relatedPerson is null)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.RelatedPersonNotFoundById);
                throw new NotFoundException(string.Format(message, request.RelatedPersonId), true);
            }

            person.RelatedPersons.Remove(relatedPerson);

            return new Unit();
        }
    }
}
