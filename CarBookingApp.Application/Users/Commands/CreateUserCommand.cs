using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Commands;

public class CreateUserCommand : IRequest<UserDTO>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [EnumDataType(typeof(Gender))]
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; } 
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<CreateUserCommand, User>(request);
        
        var createdUser = await _unitOfWork.EntityRepository.AddAsync(user);
        await _unitOfWork.Save();
        
        var userDto = _mapper.Map<User, UserDTO>(createdUser);
        return userDto;
    }
}