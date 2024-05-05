using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Commands;

public class UpdateUserCommand : IRequest<UserDTO>
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; } 
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDTO>
{
    private readonly IUnitOfWork<User> _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUnitOfWork<User> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<UpdateUserCommand, User>(request);

        var updatedUser = await _unitOfWork.EntityRepository.UpdateAsync(user);
        await _unitOfWork.Save();
        
        var userDto = _mapper.Map<User, UserDTO>(updatedUser);
        return userDto;
    }
}