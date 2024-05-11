using CarBookingApp.Application.Vehicles.Responses;
using FluentValidation;

namespace CarBookingApp.Application.Vehicles.Validations;

public class VehicleDTOValidator : AbstractValidator<VehicleDTO>
{
    public VehicleDTOValidator()
    {
        RuleFor(x => x.Vender)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Model)
            .NotEmpty()
            .MaximumLength(50);
    }
}