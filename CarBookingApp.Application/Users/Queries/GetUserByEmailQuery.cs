using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;

using AutoMapper;
using MediatR;

namespace CarBookingApp.Application.Users.Queries;

public class GetUserByEmailQuery : IRequest<UserDTO>
{
    public string Email { get; set; }
}

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;


    public GetUserByEmailQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {

        var user = await _repository.GetByPredicate<User>(u => u.Email == request.Email);
        UserDTO userDTO;
        if (user.First() is Driver driver)
        {
            userDTO = _mapper.Map<Driver, UserDTO>(driver);
        }
        else
        {
            userDTO = _mapper.Map<User, UserDTO>(user.First());
        }
        
        return userDTO;
    }
}