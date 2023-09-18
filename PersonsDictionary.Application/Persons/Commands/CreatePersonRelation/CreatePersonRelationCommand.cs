using Domain.Enums;
using Application.Abstractions.Messaging;
using Application.Models;

namespace Application.Persons.Commands.CreatePersonRelation
{
    public sealed record CreatePersonRelationCommand(
        int PersonId,
        int RelatedPersonId,
        RelationType RelatedType) : ICommand<RelatedPersonsModel>;
}
