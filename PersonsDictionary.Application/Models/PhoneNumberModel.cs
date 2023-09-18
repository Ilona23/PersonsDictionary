using Domain.Enums;

namespace Application.Models
{
    public class PhoneNumberModel
    {
        public string Number { get; set; }
        public PhoneNumberType NumberType { get; set; }
    }
}
