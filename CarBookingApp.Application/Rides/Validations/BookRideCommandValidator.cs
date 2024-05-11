using CarBookingApp.Application.Rides.Commands;
using FluentValidation;

namespace CarBookingApp.Application.Rides.Validations;

public class BookRideCommandValidator : AbstractValidator<BookRideCommand>
{
    public BookRideCommandValidator()
    {
        RuleFor(x => x.RideId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.PassengerId)
            .NotEmpty()
            .GreaterThan(0);
    }
}