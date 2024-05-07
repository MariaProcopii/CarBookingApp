namespace CarBookingApp.Application.Drivers.Responses;

public class DriverDTO
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; }
    public required DateTime DateOfBirth { get; set; } 
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; } 
    public required int YearsOfExperience { get; set; }
}