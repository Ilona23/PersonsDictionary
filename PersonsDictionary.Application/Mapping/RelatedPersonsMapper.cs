using Application.Models;
using Domain.Entities;

namespace Application.Mapping
{
    public class RelatedPersonsMapper : BaseMapper<PersonRelation, RelatedPersonsModel>
    {
        public override RelatedPersonsModel MapToModel(PersonRelation entity)
        {
            return new RelatedPersonsModel
            {
                PersonId = entity.PersonId,
                RelatedPersonId = entity.RelatedPersonId,
                RelationType = entity.RelationType
            };
        }

        public override PersonRelation MapToEntity(RelatedPersonsModel model)
        {
            return new PersonRelation
            {
                PersonId = model.PersonId,
                RelatedPersonId = model.RelatedPersonId,
                RelationType = model.RelationType
            };
        }

        public override List<RelatedPersonsModel> MapToModelList(IEnumerable<PersonRelation> entities)
        {
            return entities.Select(MapToModel).ToList();
        }

        public override List<PersonRelation> MapToEntityList(IEnumerable<RelatedPersonsModel> models)
        {
            return models.Select(MapToEntity).ToList();
        }
    }
}
