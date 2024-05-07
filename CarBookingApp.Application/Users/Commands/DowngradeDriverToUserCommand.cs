using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Commands;

public record DowngradeDriverToUserCommand(int Id) : IRequest<UserDTO>;

public class DowngradeDriverToUserCommandHandler : IRequestHandler<DowngradeDriverToUserCommand, UserDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public DowngradeDriverToUserCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(DowngradeDriverToUserCommand request, CancellationToken cancellationToken)
    {
        var driver = await _repository.GetByIdAsync<Driver>(request.Id);
        var user = _mapper.Map<Driver, User>(driver);

        await _repository.DeleteAsync<Driver>(request.Id);
        await _repository.Save();
        var createdUser = await _repository.AddAsync<User>(user);
        await _repository.Save();

        var userDto = _mapper.Map<User, UserDTO>(createdUser);
        return userDto;
    }
}