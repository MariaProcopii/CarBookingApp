using AutoMapper;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class RideProfile : Profile
{
    public RideProfile()
    {
        CreateMap<Ride, RideDTO>()
            .ForMember(dest => dest.DestinationFrom, opt =>
                opt.MapFrom(src => src.DestinationFrom.Name))
            .ForMember(dest => dest.DestinationTo, opt =>
                opt.MapFrom(src => src.DestinationTo.Name));
    }
}