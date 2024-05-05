using AutoMapper;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Profiles;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<User, UserDTO>();
        CreateMap<CreateUserCommand, User>(); 
        CreateMap<UpdateUserCommand, User>();
    }
}