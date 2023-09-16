using Application.Abstractions.Messaging;
using Application.Persons.Queries.GetPersonById;
using Domain.Enums;

namespace Application.Persons.Queries.GetPersons;

public class GetPersonsQuery : IQuery<GetPersonsResponse>, IPagedQuery
{
    public string? SearchTerm { get; set; }

    public DateTime? BirthDate { get; set; }

    public int? CityId { get; set; }

    public string? PhoneNumber { get; set; }

    public int? RelatedPersonId { get; set; }

    public Gender? Gender { get; set; }

    public PhoneNumberType? PhoneNumberType { get; set; }

    public RelationType? RelationType { get; set; }

    public int? PageSize { get; set; }

    public int? Page { get; set; }

    public SortOrder? SortOrder { get; set; }

    public string? SortBy { get; set; }
}

public class GetPersonsResponse : IPagedQueryResult
{
    public int TotalCount { get; set; }

    public IEnumerable<PersonResponse> Items { get; set; }
}
