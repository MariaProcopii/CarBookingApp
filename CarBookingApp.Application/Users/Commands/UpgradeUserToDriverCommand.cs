using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Commands;

public class UpgradeUserToDriverCommand : IRequest<UserDTO>
{
    public int Id;
    public int YearsOfExperience { get; set; }
}

public class UpgradeToDriverCommandHandler : IRequestHandler<UpgradeUserToDriverCommand, UserDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public UpgradeToDriverCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(UpgradeUserToDriverCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync<User>(request.Id);
        var driver = _mapper.Map<User, Driver>(user);
        driver.YearsOfExperience = request.YearsOfExperience;
        
        await _repository.DeleteAsync<User>(request.Id);
        var createdDriver = await _repository.AddAsync<User>(driver);
        await _repository.Save();
        
        var userDto = _mapper.Map<User, UserDTO>(createdDriver);
        return userDto;
    }
}