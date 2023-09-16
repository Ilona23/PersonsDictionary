using Domain.Enums;

namespace Domain.Entities
{
    public class PersonRelation
    {
        public PersonRelation()
        {

        }

        public PersonRelation(Person person, Person relatedPerson, RelationType relationType)
        {
            Person = person;
            RelatedPerson = relatedPerson;
            RelationType = relationType;
        }

        public int PersonId { get; set; }
        public int RelatedPersonId { get; set; }
        public RelationType RelationType { get; set; }
        public virtual Person Person { get; set; }
        public virtual Person RelatedPerson { get; set; }
    }
}