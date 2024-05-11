using CarBookingApp.Application.Rides.Commands;
using FluentValidation;

namespace CarBookingApp.Application.Rides.Validations;


public class CreateRideCommandValidator : AbstractValidator<CreateRideCommand>
{
    public CreateRideCommandValidator()
    {
        RuleFor(x => x.DateOfTheRide)
            .NotEmpty()
            .Must(BeInTheFuture).WithMessage("DateOfTheRide must be in the future.");

        RuleFor(x => x.DestinationFrom)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.DestinationTo)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.TotalSeats)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.OwnerId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.RideDetail)
            .NotNull()
            .SetValidator(new RideDetailDTOValidator());
    }

    private bool BeInTheFuture(DateTime date)
    {
        return date > DateTime.Now;
    }
}