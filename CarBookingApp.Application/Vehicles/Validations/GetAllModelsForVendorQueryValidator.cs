using CarBookingApp.Application.Vehicles.Queries;
using FluentValidation;

namespace CarBookingApp.Application.Vehicles.Validations;

public class GetAllModelsForVendorQueryValidator : AbstractValidator<GetAllModelsForVendorQuery>
{
    public GetAllModelsForVendorQueryValidator()
    {
        RuleFor(x => x.Vendor)
            .NotEmpty()
            .MaximumLength(50);
    }
}