using Domain.Entities;
using Domain.Enums;

namespace Application.Models
{
    public class RelatedPersonsModel
    {
        public int PersonId { get; set; }
        public int RelatedPersonId { get; set; }
        public RelationType RelationType { get; set; }
        public virtual Person Person { get; set; }
        public virtual Person RelatedPerson { get; set; }
    }
}
