using Domain.Enums;

namespace Domain.Entities
{
    public class PhoneNumber : Entity
    {
        public PhoneNumber()
        {
        }

        public string Number { get; set; }
        public PhoneNumberType NumberType { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
