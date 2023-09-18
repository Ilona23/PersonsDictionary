using FluentValidation;
using Application.Constants;
using Application.Services;
using Domain.Enums;

namespace Application.Persons.Commands.UpdatePerson
{
    public class UpdatePersonValidator : AbstractValidator<UpdatePersonCommand>
    {
        private readonly string latOrGeoAlphabetsRegex = @"^((?=[\p{IsBasicLatin}\s]*$|[\p{IsGeorgian}\s]*$)[\p{L}\s]*)$";

        private readonly IResourceManagerService _resourceManagerService;

        public UpdatePersonValidator() { }

        public UpdatePersonValidator(IResourceManagerService resourceManagerService)
        {
            _resourceManagerService = resourceManagerService;

            RuleFor(x => x.FirstName)
                .NotNull()
                .WithMessage(GetResourceString(ValidationMessages.FirstNameIsRequired))
                .NotEmpty()
                .Length(2, 50)
                .WithMessage(GetResourceString(ValidationMessages.FirstNameInvalidLength))
                .Matches(latOrGeoAlphabetsRegex)
                .WithMessage(GetResourceString(ValidationMessages.FirstNameInvalidAlphabets));

            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage(GetResourceString(ValidationMessages.LastNameIsRequired))
                .NotEmpty()
                .Length(2, 50)
                .WithMessage(GetResourceString(ValidationMessages.LastNameInvalidLength))
                .Matches(latOrGeoAlphabetsRegex)
                .WithMessage(GetResourceString(ValidationMessages.LastNameInvalidAlphabets));

            RuleFor(x => x.PhoneNumbers)
                .NotNull()
                .WithMessage("Phone numbers cannot be null")
                .Must(x => x.Any())
                .WithMessage(GetResourceString(ValidationMessages.AtLeastOnePhoneNumberMustBeProvided));

            RuleForEach(x => x.PhoneNumbers)
                .ChildRules(phoneNumber =>
                {
                    phoneNumber.RuleFor(x => x.Number)
                        .Length(4, 50)
                        .WithMessage(GetResourceString(ValidationMessages.NumberInvalidLength));

                    phoneNumber.RuleFor(x => x.NumberType)
                        .Must(x => Enum.TryParse<PhoneNumberType>(x.ToString(), out _))
                        .WithMessage(GetResourceString(ValidationMessages.NumberInvalidType));
                });

        }

        private string GetResourceString(string key)
        {
            return _resourceManagerService.GetString(key);
        }
    }
}
