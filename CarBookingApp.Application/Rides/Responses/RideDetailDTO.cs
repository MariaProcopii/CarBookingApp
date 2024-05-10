namespace CarBookingApp.Application.Rides.Responses;

public class RideDetailDTO
{
    public string PickUpSpot { get; set; }
    public decimal Price { get; set; }
    public List<String> Facilities { get; set; } = [];
}