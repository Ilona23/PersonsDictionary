using MediatR;
using Domain.Exceptions;
using System.Net;
using Domain.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities;
using Application.Models;

namespace Application.Persons.Commands.CreatePerson
{
    public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, PersonModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _repository;
        private readonly IDtoToEntityMapper _dtoToEntityMapper;
        private readonly IMapper<Person, PersonModel> _personMapper;

        public CreatePersonHandler(IPersonRepository repository,
                                   IUnitOfWork unitOfWork,
                                   IDtoToEntityMapper dtoToEntityMapper,
                                   IMapper<Person, PersonModel> personMapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _dtoToEntityMapper = dtoToEntityMapper;
            _personMapper = personMapper;
        }

        public async Task<PersonModel> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var existingPerson = await _repository.GetPersonByPersonalIdAsync(request.PersonalId, cancellationToken);
            if (existingPerson != null)
            {
                throw new HttpException($"Person with PersonalId: {request.PersonalId} already exists.", HttpStatusCode.Conflict);
            }

            var person = _dtoToEntityMapper.MapToEntity(request);

            await _repository.InsertAsync(person, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return _personMapper.MapToModel(person);
        }
    }
}
