using CarBookingApp.Application.Users.Commands;
using FluentValidation;

namespace CarBookingApp.Application.Users.Validations;

public class ApproveUserForRideCommandValidator : AbstractValidator<ApproveUserForRideCommand>
{
    public ApproveUserForRideCommandValidator()
    {
        RuleFor(x => x.RideId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.PassengerId)
            .NotEmpty()
            .GreaterThan(0);
    }
}
