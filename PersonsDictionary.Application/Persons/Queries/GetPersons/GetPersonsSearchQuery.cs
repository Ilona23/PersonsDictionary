using Application.Models;
using Domain.Abstractions;
using MediatR;

namespace Application.Persons.Queries.GetPersons
{
    public sealed record GetPersonsSearchQuery(
        string? QuickSearch,
        string? FirstName,
        string? LastName,
        string? PersonalId,
        int? Page,
        int? PageSize) : IRequest<PagedResult<PersonDetailedModel>>;
}
