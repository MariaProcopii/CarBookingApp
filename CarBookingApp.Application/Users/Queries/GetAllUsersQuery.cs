using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Queries;

public record GetAllUsersQuery() : IRequest<List<UserDTO>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _repository.GetAllAsync<User>();
        var userDTOs = new List<UserDTO>();
        
        foreach (var user in users)
        {
            if (user is Driver driver)
            {
                userDTOs.Add(_mapper.Map<Driver, UserDTO>(driver));
            }
            else
            {
                userDTOs.Add(_mapper.Map<User, UserDTO>(user));
            }
        }

        return userDTOs;
    }
}