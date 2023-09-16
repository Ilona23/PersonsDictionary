using Application.Abstractions.Messaging;
using Domain.Models;

namespace Application.Persons.Queries.GetPersons;
public sealed record GetPersonsRelationsQuery() : IQuery<IQueryable<PersonsRelationsModel>>;

