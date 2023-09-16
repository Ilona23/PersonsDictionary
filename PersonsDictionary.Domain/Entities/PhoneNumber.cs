using Domain.Enums;
using Domain.Models;

namespace Domain.Entities
{
    public class PhoneNumber : Entity
    {
        public PhoneNumber()
        {
        }

        public PhoneNumber(PhoneNumberModel model)
        {
            Number = model.Number;
            NumberType = model.NumberType;
            CreatedDate = DateTime.Now;
        }

        public void Update(UpdatePhoneNumberModel model)
        {
            Number = model.Number;
            NumberType = model.NumberType;
            UpdatedDate = DateTime.Now;
        }

        public string Number { get; set; }
        public PhoneNumberType NumberType { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
