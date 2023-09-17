using Domain.Enums;

namespace Domain.Models
{
    public class PersonsRelationsModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public RelationType RelationType { get; set; }
        public int Count { get; set; }
    }
}
