using CarBookingApp.Domain.Enum;

namespace CarBookingApp.Domain.Model;

public class UserRide : Entity
{
    public int PassengerId { get; set; }
    public User Passenger { get; set; }
    public int RideId { get; set; }
    public Ride Ride { get; set; }
    public BookingStatus BookingStatus { get; set; }
    public RideStatus RideStatus { get; set; }
    public ReviewDialog ReviewDialog { get; set; }

}