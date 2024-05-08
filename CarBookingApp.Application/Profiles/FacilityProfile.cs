using AutoMapper;
using CarBookingApp.Application.Abstractions.Facilities.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class FacilityProfile : Profile
{
    public FacilityProfile()
    {
        CreateMap<Facility, FacilityDTO>();
    }
}