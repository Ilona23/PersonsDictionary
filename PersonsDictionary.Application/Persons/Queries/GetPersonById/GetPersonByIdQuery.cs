using Application.Abstractions.Messaging;
using Application.Models;

namespace Application.Persons.Queries.GetPersonById
{
    public sealed record GetPersonByIdQuery(int Id) : IQuery<PersonDetailedModel>;
}
