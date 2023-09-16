//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Application.PersonManagement.Queries;
//using Application.PersonManagement.Records;
//using Application.Persons.Models;
//using Domain.Interfaces;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using Microsoft.EntityFrameworkCore;

//namespace Application.Persons.Handlers
//{
//    public class GetPersonsHandler : IRequestHandler<GetPersonsQuery, GetPersonsResponse>
//    {
//        private readonly IPersonRepository _repository;
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public GetPersonsHandler(IPersonRepository repository, IHttpContextAccessor httpContextAccessor)
//        {
//            _repository = repository;
//            _httpContextAccessor = httpContextAccessor;
//        }

//        public async Task<GetPersonsResponse> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
//        {
//            request.SearchTerm = request.SearchTerm?.Trim().ToLower();

//            var baseQuery = _repository.Query()
//                .And(request.SearchTerm, x => x.FirstName.Contains(request.SearchTerm) || x.LastName.Contains(request.SearchTerm) || x.PersonalId.Contains(request.SearchTerm))
//                .And(request.BirthDate, x => x.BirthDate == request.BirthDate)
//                .And(request.PhoneNumber, x => x.PhoneNumbers.Any(e => e.Number == request.PhoneNumber))
//                .And(request.PhoneNumberType, x => x.PhoneNumbers.Any(e => e.NumberType == request.PhoneNumberType))
//                .And(request.Gender, x => x.Gender == request.Gender);

//            var totalCount = baseQuery.Count();

//            var persons = await baseQuery
//                .SortAndPage(request)
//                .Select(x => new PersonRecord(
//                    x.Id,
//                    x.FirstName,
//                    x.LastName,
//                    x.PersonalId,
//                    $"{x.BirthDate:dd-MM-yyyy}",
//                    x.GetImage(_httpContextAccessor),
//                    $"{x.Gender}",
//                    x.RelatedPersons.Select(p => new RelatedPersonRecord(
//                            p.RelatedPerson.FirstName,
//                            p.RelatedPerson.LastName,
//                            p.RelatedPerson.PersonalId,
//                            $"{p.RelatedPerson.BirthDate:dd-MM-yyyy}",
//                            p.RelatedPerson.GetImage(_httpContextAccessor),
//                            $"{p.RelatedPerson.Gender}",
//                            $"{p.RelationType}")),
//                    x.RelatedToPersons.Select(p => new RelatedPersonRecord(
//                            p.Person.FirstName,
//                            p.Person.LastName,
//                            p.Person.PersonalId,
//                            $"{p.Person.BirthDate:dd-MM-yyyy}",
//                            p.Person.GetImage(_httpContextAccessor),
//                            $"{p.Person.Gender}",
//                            $"{p.RelationType}")),
//                    x.PhoneNumbers.Select(p => new PhoneNumberModel { Number = p.Number, NumberType = p.NumberType })
//                ))
//                .ToListAsync(cancellationToken);

//            return new GetPersonsResponse
//            {
//                TotalCount = totalCount,
//                Items = persons
//            };
//        }
//    }
//}
