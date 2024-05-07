using CarBookingApp.Domain.Enum;


namespace CarBookingApp.Domain.Model;

public class Destination : Entity
{
    public required string Name { get; set; }
    public required Region Region { get; set; }
}