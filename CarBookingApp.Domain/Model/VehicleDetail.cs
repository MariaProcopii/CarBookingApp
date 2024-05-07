namespace CarBookingApp.Domain.Model;

public class VehicleDetail : Entity
{
    public required int ManufactureYear { get; set; }
    public required string RegistrationNumber { get; set; }
    public Vehicle Vehicle { get; set; }
}