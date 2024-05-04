namespace CarBookingApp.Domain.Model;

public class Facility : Entity
{
    public int Id { get; set; }
    public required string FacilityType { get; set; }
}