using Application.Abstractions.Messaging;
using Domain.Models;

namespace Application.Persons.Queries.GetPersonsRelations
{
    public sealed record GetPersonsRelationsQuery() : IQuery<IQueryable<PersonRelationModel>>;
}
