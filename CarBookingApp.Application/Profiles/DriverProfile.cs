using AutoMapper;
using CarBookingApp.Application.Drivers.Commands;
using CarBookingApp.Application.Drivers.Responses;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class DriverProfile : Profile
{
    public DriverProfile()
    {
        CreateMap<Driver, DriverDTO>();
        CreateMap<UpgradeToDriverCommand, Driver> ();
    }
}