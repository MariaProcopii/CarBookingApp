using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Domain.Enum;
using FluentValidation;

namespace CarBookingApp.Application.Users.Validations;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Gender).NotEmpty().IsEnumName(typeof(Gender));
        RuleFor(x => x.DateOfBirth).NotEmpty().Must(BeAValidDate);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^[0-9]*$");
    }
    
    private bool BeAValidDate(DateTime date)
    {
        return date < DateTime.Now;
    }
}