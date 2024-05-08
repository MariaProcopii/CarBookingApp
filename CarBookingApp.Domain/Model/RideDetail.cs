namespace CarBookingApp.Domain.Model;

public class RideDetail : Entity
{
    public required string PickUpSpot { get; set; }
    public required decimal Price { get; set; }
    public List<Facility> Facilities { get; set; } = [];
}