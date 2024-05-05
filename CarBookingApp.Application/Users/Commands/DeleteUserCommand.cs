using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Commands;

public class DeleteUserCommand : IRequest<UserDTO>
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDTO>
{
    private readonly IUnitOfWork<User> _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(IUnitOfWork<User> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var deletedUser = await _unitOfWork.EntityRepository.DeleteAsync(request.Id);
        await _unitOfWork.Save();
        
        var userDto = _mapper.Map<User, UserDTO>(deletedUser);
        return userDto;
    }
}