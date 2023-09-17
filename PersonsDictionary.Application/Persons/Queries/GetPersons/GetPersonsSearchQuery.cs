using Domain.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Persons.Queries.GetPersons
{
    public sealed record GetPersonsSearchQuery(
        string? QuickSearch,
        string? FirstName,
        string? LastName,
        string? PersonalId,
        int? Page,
        int? PageSize) : IRequest<PagedResult<Person>>;
}
