using MediatR;
using Domain.Enums;
using Application.Abstractions.Messaging;

namespace Application.Persons.Commands.CreatePersonRelation
{
    public sealed record CreatePersonRelationCommand(
        int PersonId,
        int RelatedPersonId,
        RelationType RelatedType) : ICommand<Unit>;
}
