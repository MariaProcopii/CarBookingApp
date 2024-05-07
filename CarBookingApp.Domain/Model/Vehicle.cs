namespace CarBookingApp.Domain.Model;

public class Vehicle : Entity
{
    public int Id { get; set; }
    public required string Vender { get; set; }
    public required string Model { get; set; }
}