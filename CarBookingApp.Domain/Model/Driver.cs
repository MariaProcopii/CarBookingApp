namespace CarBookingApp.Domain.Model;

public class Driver : User
{
    public required int YearsOfExperience { get; set; }
    public List<Ride> CreatedRides { get; set; } = [];
    public List<Vehicle> Vehicles { get; set; } = [];
}