using CarBookingApp.Application.Auth.Command;
using CarBookingApp.Domain.Enum;
using FluentValidation;

namespace CarBookingApp.Application.Auth.Validations;

public class SignUpUserCommandValidator : AbstractValidator<SignUpUserCommand>
{
    public SignUpUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(30);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Gender).NotEmpty().IsEnumName(typeof(Gender));
        RuleFor(x => x.DateOfBirth).NotEmpty().Must(BeAValidDate)
            .WithMessage("birthday: The user must be at least 18 years old.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("password: Password must be at least 8 characters long.")
            .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)[A-Za-z\d]{8,}$")
            .WithMessage("password: Password must contain at least one uppercase letter, one lowercase letter and one digit.");
    }
    
    private bool BeAValidDate(DateTime dateOfBirth)
    {
        DateTime minimumDateOfBirth = DateTime.Now.Date.AddYears(-18);
        return dateOfBirth <= minimumDateOfBirth;
    }
}