using Microsoft.AspNetCore.Http;
using Domain.Enums;

namespace Domain.Entities
{
    public class Person : Entity
    {
        public Person()
        {
            RelatedPersons = new List<PersonRelation>();
            RelatedToPersons = new List<PersonRelation>();
            PhoneNumbers = new List<PhoneNumber>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public DateTime BirthDate { get; set; }
        public int CityId { get; set; }
        public Gender Gender { get; set; }
        public virtual ICollection<PersonRelation> RelatedPersons { get; set; }
        public virtual ICollection<PersonRelation> RelatedToPersons { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

        public string GetImage(IHttpContextAccessor httpContextAccessor)
        {
            string fileName = $"{FirstName}_{LastName}_{Id}.jpg";

            var imagePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            var result = File.Exists(imagePath);

            if (result)
            {
                return $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}/images/{fileName}";
            }

            return null;
        }
    }
}
