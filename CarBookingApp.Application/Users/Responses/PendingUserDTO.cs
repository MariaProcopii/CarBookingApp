using CarBookingApp.Application.Rides.Responses;

namespace CarBookingApp.Application.Users.Responses;

public class PendingUserDTO
{
    public UserDTO UserInfo { get; set; }
    public RideCreatedInfoDTO RideInfo { get; set; }
}