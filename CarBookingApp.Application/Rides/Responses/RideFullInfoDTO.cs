using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Rides.Responses;

public class RideFullInfoDTO
{
    public int Id { get; set; }
    public DateTime DateOfTheRide { get; set; } 
    public string DestinationFrom { get; set; }
    public string DestinationTo { get; set; }
    public int TotalSeats { get; set; }
    public required UserDTO Owner { get; set; }
    public RideDetailDTO RideDetail { get; set; }
    public List<UserDTO> Passengers { get; set; } = [];
}