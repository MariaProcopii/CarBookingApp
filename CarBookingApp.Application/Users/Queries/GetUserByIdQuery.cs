using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Queries;

public record GetUserByIdQuery(int UserId) : IRequest<UserDTO>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;


    public GetUserByIdQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {

        var user = await  _repository.GetByIdAsync<User>(request.UserId);
        UserDTO userDTO;
        if (user is Driver driver)
        {
            userDTO = _mapper.Map<Driver, UserDTO>(driver);
        }
        else
        {
            userDTO = _mapper.Map<User, UserDTO>(user);
        }
        
        return userDTO;
    }
}