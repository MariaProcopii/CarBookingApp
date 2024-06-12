namespace CarBookingApp.Application.Rides.Responses;

public class RideShortInfoDTO
{
    public int Id { get; set; }
    public DateTime DateOfTheRide { get; set; } 
    public string DestinationFrom { get; set; }
    public string DestinationTo { get; set; }
    public int TotalSeats { get; set; }
    public string OwnerName { get; set; }
    public string OwnerGender { get; set; }
    public decimal Price { get; set; }
}