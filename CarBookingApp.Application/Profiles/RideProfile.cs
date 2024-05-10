using AutoMapper;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class RideProfile : Profile
{
    public RideProfile()
    {
        CreateMap<Ride, RideWithRideDetailsInfoDTO>()
            .ForMember(dest => dest.DestinationFrom, opt =>
                opt.MapFrom(src => src.DestinationFrom.Name))
            .ForMember(dest => dest.DestinationTo, opt =>
                opt.MapFrom(src => src.DestinationTo.Name));
        
        CreateMap<Ride, RideShortInfoDTO>()
            .ForMember(dest => dest.DestinationFrom, opt =>
                opt.MapFrom(src => src.DestinationFrom.Name))
            .ForMember(dest => dest.DestinationTo, opt =>
                opt.MapFrom(src => src.DestinationTo.Name));
        
        CreateMap<Ride, RideFullInfoDTO>()
            .ForMember(dest => dest.DestinationFrom, opt =>
                opt.MapFrom(src => src.DestinationFrom.Name))
            .ForMember(dest => dest.DestinationTo, opt =>
                opt.MapFrom(src => src.DestinationTo.Name));
    }
}