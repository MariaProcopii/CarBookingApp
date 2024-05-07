using CarBookingApp.Application.Vehicles.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.VehicleDetails.Responses;

public class VehicleDetailDTO
{
    public int ManufactureYear { get; set; }
    public string RegistrationNumber { get; set; }
    public VehicleDTO Vehicle { get; set; }
}