using MediatR;
using Domain.Entities;
using Domain.Exceptions;
using System.Net;
using Domain.Abstractions;

namespace Application.Persons.Commands.CreatePerson
{
    public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _repository;
        private readonly IDTOToEntityMapper _DTOToEntityMapper;

        public CreatePersonHandler(IPersonRepository repository,
                                   IUnitOfWork unitOfWork,
                                   IDTOToEntityMapper DTOToEntityMapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _DTOToEntityMapper = DTOToEntityMapper;
        }

        public async Task<Unit> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var existingPerson = await _repository.FirstOrDefaultAsync(x => x.PersonalId == request.PersonalId);
            if (existingPerson != null)
            {
                throw new HttpException($"Person with PersonalId: {request.PersonalId} already exists.", HttpStatusCode.AlreadyReported);
            }

            IEnumerable<PhoneNumber> convertPhoneNumbers = request.PhoneNumbers.Select(x => new PhoneNumber
            {
                Number = x.Number,
                NumberType = x.NumberType
            });

            var per = _DTOToEntityMapper.ConvertDTOToEntity(request);

            var person = new Person()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PersonalId = request.PersonalId,
                BirthDate = request.BirthDate,
                CityId = request.CityId,
                Gender = request.Gender,
                PhoneNumbers = convertPhoneNumbers.ToArray(),
            };

            await _repository.InsertAsync(person);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new Unit();
        }
    }
}
