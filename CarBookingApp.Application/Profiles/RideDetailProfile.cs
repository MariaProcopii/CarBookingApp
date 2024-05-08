using AutoMapper;
using CarBookingApp.Application.RideDetails.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class RideDetailProfile : Profile
{
    public RideDetailProfile()
    {
        CreateMap<RideDetail, RideDetailDTO>();
        CreateMap<RideDetailDTO, RideDetail>();
    }
}