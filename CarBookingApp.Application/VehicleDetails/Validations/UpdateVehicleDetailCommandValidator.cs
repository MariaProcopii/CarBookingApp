using CarBookingApp.Application.VehicleDetails.Commands;
using CarBookingApp.Application.Vehicles.Validations;
using FluentValidation;

namespace CarBookingApp.Application.VehicleDetails.Validations;

public class UpdateVehicleDetailCommandValidator : AbstractValidator<UpdateVehicleDetailCommand>
{
    public UpdateVehicleDetailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Vehicle)
            .NotNull()
            .SetValidator(new VehicleDTOValidator());

        RuleFor(x => x.ManufactureYear)
            .NotEmpty()
            .InclusiveBetween(1900, 2100);

        RuleFor(x => x.RegistrationNumber)
            .NotEmpty()
            .Matches("^[A-Z]{3} [0-9]{3}}$")
            .WithMessage("Invalid registration number format (Example: ABC-123).");
    }
}