using AutoMapper;
using CarBookingApp.Application.VehicleDetails.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class VehicleDetailProfile : Profile
{
    public VehicleDetailProfile()
    {
        CreateMap<VehicleDetail, VehicleDetailDTO>();
        CreateMap<VehicleDetailDTO, Vehicle>();
    }
}