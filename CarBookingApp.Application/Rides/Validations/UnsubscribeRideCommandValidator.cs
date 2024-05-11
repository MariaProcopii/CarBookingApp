using CarBookingApp.Application.Rides.Commands;
using FluentValidation;

namespace CarBookingApp.Application.Rides.Validations;

public class UnsubscribeRideCommandValidator : AbstractValidator<UnsubscribeFromRideCommand>
{
    public UnsubscribeRideCommandValidator()
    {
        RuleFor(x => x.RideId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.PassengerId)
            .NotEmpty()
            .GreaterThan(0);
    }
}