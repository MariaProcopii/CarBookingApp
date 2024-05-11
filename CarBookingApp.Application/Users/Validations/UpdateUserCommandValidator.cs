using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Domain.Enum;
using FluentValidation;

namespace CarBookingApp.Application.Users.Validations;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Gender).NotEmpty().IsEnumName(typeof(Gender));
        RuleFor(x => x.DateOfBirth).NotEmpty().Must(BeAValidDate)
            .WithMessage("The user must be at least 18 years old.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^[0-9]*$");
    }

    private bool BeAValidDate(DateTime dateOfBirth)
    {
        DateTime minimumDateOfBirth = DateTime.Now.Date.AddYears(-18);
        return dateOfBirth <= minimumDateOfBirth;
    }
}