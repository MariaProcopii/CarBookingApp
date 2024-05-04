namespace CarBookingApp.Domain.Model;

public class Ride : Entity
{
    public int Id { get; set; }
    public required DateTime DateOfTheRide { get; set; } 
    public required Destination DestinationFrom { get; set; }
    public required Destination DestinationTo { get; set; }
    public required int TotalSeats { get; set; }
    public required Driver Owner { get; set; }
    public RideDetail RideDetail { get; set; }
    public List<User> Passengers { get; set; } = [];
}