using AutoMapper;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class RideDetailProfile : Profile
{
    public RideDetailProfile()
    {
        CreateMap<RideDetail, RideDetailDTO>()
            .ForMember(dest => dest.Facilities,
                opt => 
                    opt.MapFrom(src => src.Facilities.Select(f => f.FacilityType).ToList()));
        CreateMap<RideDetailDTO, RideDetail>();
    }
}