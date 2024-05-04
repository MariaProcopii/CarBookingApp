namespace CarBookingApp.Domain.Model;

public class RideDetail : Entity
{
    public int Id { get; set; }
    public Ride Ride { get; set; }
    public string? PickUpSpot { get; set; }
    public decimal Price { get; set; }
    public List<Facility> Facilities { get; set; } = [];
}