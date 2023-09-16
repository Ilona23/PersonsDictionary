namespace Application.Persons.Queries.GetPersonById;

public record RelatedPersonRecord(
    string FirstName,
    string LastName,
    string PersonalId,
    string BirthDate,
    string Image,
    string Gender,
    string RelationType);
