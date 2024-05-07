using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Commands;

public class UpdateUserCommand : IRequest<UserDTO>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int? YearsOfExperience { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {

        var existingUser = await _repository.GetByIdAsync<User>(request.Id);
        var updateUser = _mapper.Map<UpdateUserCommand, User>(request, existingUser);
        if (existingUser is Driver existingDriver)
        { 
            updateUser = _mapper.Map(request, existingDriver);
        }

        var updatedUser = await _repository.UpdateAsync(updateUser);
        await _repository.Save();

        var userDto = _mapper.Map<User, UserDTO>(updatedUser);
        return userDto;
    }
}