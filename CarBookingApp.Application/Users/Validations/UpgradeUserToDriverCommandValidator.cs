using CarBookingApp.Application.Users.Commands;
using FluentValidation;

namespace CarBookingApp.Application.Users.Validations;

public class UpgradeUserToDriverCommandValidator : AbstractValidator<UpgradeUserToDriverCommand>
{
    public UpgradeUserToDriverCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.YearsOfExperience)
            .NotEmpty()
            .GreaterThan(0)
            .LessThanOrEqualTo(50);
    }
}