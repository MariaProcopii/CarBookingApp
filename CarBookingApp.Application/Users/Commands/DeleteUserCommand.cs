using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Commands;

public record DeleteUserCommand(int UserId) : IRequest<UserDTO>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(IMapper mapper, IRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }


    public async Task<UserDTO> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var deletedUser = await _repository.DeleteAsync<User>(request.UserId);
        await _repository.Save();
        
        var userDto = _mapper.Map<User, UserDTO>(deletedUser);
        return userDto;
    }
}