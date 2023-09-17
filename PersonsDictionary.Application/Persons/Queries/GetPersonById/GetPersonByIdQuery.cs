using Application.Abstractions.Messaging;

namespace Application.Persons.Queries.GetPersonById
{
    public sealed record GetPersonByIdQuery(int Id) : IQuery<PersonResponse>;
}
