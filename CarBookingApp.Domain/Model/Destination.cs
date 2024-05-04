using CarBookingApp.Domain.Enum;


namespace CarBookingApp.Domain.Model;

public class Destination : Entity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Region Region { get; set; }
}