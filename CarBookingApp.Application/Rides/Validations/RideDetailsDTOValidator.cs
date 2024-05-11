using CarBookingApp.Application.Rides.Responses;
using FluentValidation;

namespace CarBookingApp.Application.Rides.Validations;

public class RideDetailDTOValidator : AbstractValidator<RideDetailDTO>
{
    public RideDetailDTOValidator()
    {
        RuleFor(x => x.PickUpSpot)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0);
    }
}