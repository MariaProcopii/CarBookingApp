using CarBookingApp.Domain.Enum;

namespace CarBookingApp.Domain.Model;

public class UserRide
{
    public int PassengerId { get; set; } 
    public int RideId { get; set; }
    public BookingStatus BookingStatus { get; set; }
    public RideStatus RideStatus { get; set; }

}