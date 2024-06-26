namespace CarBookingApp.Domain.Model;

public class Driver : User
{
    public int? YearsOfExperience { get; set; }
    public List<Ride> CreatedRides { get; set; } = [];
    public VehicleDetail VehicleDetail { get; set; }
}