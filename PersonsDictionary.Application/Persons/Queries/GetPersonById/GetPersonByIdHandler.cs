using MediatR;
using Microsoft.AspNetCore.Http;
using Domain.Exceptions;
using Application.Constants;
using Domain.Abstractions;
using Application.Services;
using Domain.Models;

namespace Application.Persons.Queries.GetPersonById
{
    public class GetPersonByIdHandler : IRequestHandler<GetPersonByIdQuery, PersonResponse>
    {
        private readonly IPersonRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IResourceManagerService _resourceManagerService;

        public GetPersonByIdHandler(IPersonRepository repository, IHttpContextAccessor httpContextAccessor, IResourceManagerService resourceManagerService)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _resourceManagerService = resourceManagerService;
        }

        public async Task<PersonResponse> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _repository.GetAsync(request.Id);

            if (person is null)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
                throw new NotFoundException(string.Format(message, request.Id), true);
            }

            return new PersonResponse(
                person.Id,
                person.FirstName,
                person.LastName,
                person.PersonalId,
                $"{person.BirthDate:dd-MM-yyyy}",
                person.GetImage(_httpContextAccessor),
                $"{person.Gender}",
                person.RelatedPersons.Select(p => new RelatedPersonRecord(
                        p.RelatedPerson.FirstName,
                        p.RelatedPerson.LastName,
                        p.RelatedPerson.PersonalId,
                        $"{p.RelatedPerson.BirthDate:dd-MM-yyyy}",
                        p.RelatedPerson.GetImage(_httpContextAccessor),
                        $"{p.RelatedPerson.Gender}",
                        $"{p.RelationType}")),
                person.RelatedToPersons.Select(p => new RelatedPersonRecord(
                        p.Person.FirstName,
                        p.Person.LastName,
                        p.Person.PersonalId,
                        $"{p.Person.BirthDate:dd-MM-yyyy}",
                        p.Person.GetImage(_httpContextAccessor),
                        $"{p.Person.Gender}",
                        $"{p.RelationType}")),
                person.PhoneNumbers.Select(p => new PhoneNumberModel
                {
                    Number = p.Number,
                    NumberType = p.NumberType
                })
            );
        }
    }
}
