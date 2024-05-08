using CarBookingApp.Domain.Enum;


namespace CarBookingApp.Domain.Model;

public class Destination : Entity
{
    public string Name { get; set; }
    public Region Region { get; set; }
}