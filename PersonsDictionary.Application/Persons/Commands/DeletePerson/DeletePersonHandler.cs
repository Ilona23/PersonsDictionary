using Application.Constants;
using Application.Services;
using MediatR;
using Domain.Exceptions;
using Domain.Abstractions;

namespace Application.Persons.Commands.DeletePerson
{
    public class DeletePersonHandler : IRequestHandler<DeletePersonCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _repository;
        private readonly IResourceManagerService _resourceManagerService;

        public DeletePersonHandler(IPersonRepository repository,
            IUnitOfWork unitOfWork,
            IResourceManagerService resourceManagerService)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _resourceManagerService = resourceManagerService;
        }

        public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _repository.GetAsync(request.Id);

            if (person is null)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
                throw new NotFoundException($"{message} {request.Id}", true);
            }

            _repository.Delete(person);
            try
            {
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
            }

            return new Unit();
        }
    }
}
