namespace CarBookingApp.Domain.Model;

public class Vehicle : Entity
{
    public required string Vender { get; set; }
    public required string Model { get; set; }
}