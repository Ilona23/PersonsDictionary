using MediatR;
using Application.Constants;
using Application.Services;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Persons.Commands.UpdatePerson;

public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonRepository _repository;
    private readonly IResourceManagerService _resourceManagerService;

    public UpdatePersonHandler(IPersonRepository repository,
                IUnitOfWork unitOfWork,
                IResourceManagerService resourceManagerService)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _resourceManagerService = resourceManagerService;
    }

    public async Task<Unit> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository.GetAsync(request.Id);

        if (person is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new NotFoundException(string.Format(message, request.Id), true);
        }

        var convertPhoneNumbers = request.PhoneNumbers.Select(x => new PhoneNumber
        {
            Number = x.Number,
            NumberType = x.NumberType
        });


        person.FirstName = request.FirstName;
        person.LastName = request.LastName;
        person.CityId = request.CityId;
        person.UpdatedDate = DateTime.Now;

        if (request.PhoneNumbers.Any())
        {
            var phoneNumbersList = new List<PhoneNumber>();

            foreach (Domain.Models.UpdatePhoneNumberModel phoneNumber in request.PhoneNumbers)
            {
                var dbPhoneNumber = person.PhoneNumbers.FirstOrDefault(x => x.Id == phoneNumber.Id);

                if (dbPhoneNumber != null)
                {
                    dbPhoneNumber.Update(phoneNumber);
                }
            }
        }

        await _repository.UpdateAsync(person);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new Unit();
    }
}