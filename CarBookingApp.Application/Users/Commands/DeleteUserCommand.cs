using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Commands;

public record DeleteUserCommand(int UserId) : IRequest<UserDTO>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var deletedUser = await _unitOfWork.EntityRepository.DeleteAsync<User>(request.UserId);
        await _unitOfWork.Save();
        
        var userDto = _mapper.Map<User, UserDTO>(deletedUser);
        return userDto;
    }
}