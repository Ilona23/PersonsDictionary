using MediatR;
using Application.Constants;
using Application.Services;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Application.Models;
using Application.Abstractions.Messaging;

namespace Application.Persons.Commands.UpdatePerson
{
    public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, PersonModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _repository;
        private readonly IResourceManagerService _resourceManagerService;
        private readonly IMapper<Person, PersonModel> _personMapper;

        public UpdatePersonHandler(IPersonRepository repository,
            IUnitOfWork unitOfWork,
            IResourceManagerService resourceManagerService,
            IMapper<Person, PersonModel> personMapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _resourceManagerService = resourceManagerService;
            _personMapper = personMapper;
        }

        public async Task<PersonModel> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = await _repository.GetPersonByIdDetailedAsync(request.Id, cancellationToken);

            if (person is null)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
                throw new NotFoundException(string.Format(message, request.Id), true);
            }

            person.FirstName = request.FirstName;
            person.LastName = request.LastName;
            person.CityId = request.CityId;
            person.UpdatedDate = DateTime.Now;

            if (request.PhoneNumbers.Any())
            {
                var phoneNumbersList = new List<PhoneNumber>();

                foreach (UpdatePhoneNumberModel phoneNumber in request.PhoneNumbers)
                {
                    var dbPhoneNumber = person.PhoneNumbers.FirstOrDefault(x => x.Id == phoneNumber.Id);

                    if (dbPhoneNumber is not null)
                    {
                        dbPhoneNumber.Number = phoneNumber.Number;
                        dbPhoneNumber.NumberType = phoneNumber.NumberType;
                        dbPhoneNumber.UpdatedDate = DateTime.Now;
                    }
                }
            }

            _repository.Update(person);
            await _unitOfWork.CommitAsync(cancellationToken);

            return _personMapper.MapToModel(person);
        }
    }
}
