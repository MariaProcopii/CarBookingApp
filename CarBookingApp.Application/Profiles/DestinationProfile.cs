using AutoMapper;
using CarBookingApp.Application.Destinations.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class DestinationProfile : Profile
{
    public DestinationProfile()
    {
        CreateMap<Destination, DestinationDTO>();
    }
}