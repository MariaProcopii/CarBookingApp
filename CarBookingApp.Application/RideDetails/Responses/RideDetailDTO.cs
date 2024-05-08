using CarBookingApp.Application.Facilities.Responses;

namespace CarBookingApp.Application.RideDetails.Responses;

public class RideDetailDTO
{
    public string PickUpSpot { get; set; }
    public decimal Price { get; set; }
    public List<FacilityDTO> Facilities { get; set; } = [];
}