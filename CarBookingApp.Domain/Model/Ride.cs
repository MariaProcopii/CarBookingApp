namespace CarBookingApp.Domain.Model;

public class Ride : Entity
{
    public required DateTime DateOfTheRide { get; set; } 
    public required Destination DestinationFrom { get; set; }
    public required Destination DestinationTo { get; set; }
    public required int AvailableSeats { get; set; }
    public required User Owner { get; set; }
    public RideDetail RideDetail { get; set; }
    public List<User> Passengers { get; set; } = [];
}