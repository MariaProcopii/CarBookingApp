using AutoMapper;
using CarBookingApp.Application.Vehicles.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class VehicleProfile : Profile
{
    public VehicleProfile()
    {
        CreateMap<Vehicle, VehicleDTO>();
    }
}