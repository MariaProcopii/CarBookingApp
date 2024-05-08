namespace CarBookingApp.Application.Rides.Responses;

public class RideDTO
{
    public int Id { get; set; }
    public DateTime DateOfTheRide { get; set; } 
    public string DestinationFrom { get; set; }
    public string DestinationTo { get; set; }
    public int TotalSeats { get; set; }
}