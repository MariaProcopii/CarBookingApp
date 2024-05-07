using AutoMapper;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<User, Driver>();
        
        CreateMap<Driver, UserDTO>();
        
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Gender, opt =>
                opt.MapFrom(src => src.Gender.ToString()));
        
        CreateMap<CreateUserCommand, User>()
            .ForMember(dest => dest.Gender, opt => 
                    opt.MapFrom(src => Enum.Parse<Gender>(src.Gender)));
        
        CreateMap<UpdateUserCommand, User>()
            .ForMember(dest => dest.Gender, opt => 
                opt.MapFrom(src => Enum.Parse<Gender>(src.Gender)));
    }
}