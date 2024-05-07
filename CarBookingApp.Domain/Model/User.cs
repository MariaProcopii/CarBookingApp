using CarBookingApp.Domain.Enum;

namespace CarBookingApp.Domain.Model;

public class User : Entity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public required DateTime DateOfBirth { get; set; } 
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; } 
    public List<Ride> BookedRides { get; set; } = [];
}