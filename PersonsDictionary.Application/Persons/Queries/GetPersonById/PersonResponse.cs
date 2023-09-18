using Application.Models;

namespace Application.Persons.Queries.GetPersonById
{
    public record PersonResponse(
        int Id,
        string FirstName,
        string LastName,
        string PersonalId,
        string BirthDate,
        string Image,
        string Gender,
        IEnumerable<RelatedPersonRecord> RelatedPersons,
        IEnumerable<RelatedPersonRecord> RelatedToPersons,
        IEnumerable<PhoneNumberModel> PhoneNumbers
    );
}
