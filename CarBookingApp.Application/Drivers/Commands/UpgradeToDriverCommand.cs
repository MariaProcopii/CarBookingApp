using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Drivers.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Drivers.Commands;

public class UpgradeToDriverCommand : IRequest<DriverDTO>
{
    public int Id;
    public int YearsOfExperience { get; set; }
}

public class UpgradeToDriverCommandHandler : IRequestHandler<UpgradeToDriverCommand, DriverDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public UpgradeToDriverCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DriverDTO> Handle(UpgradeToDriverCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync<User>(request.Id);
        
        var driver = new Driver
        {
            YearsOfExperience = request.YearsOfExperience,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender,
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Id = user.Id
        };
        await _repository.DeleteAsync<User>(request.Id);
        var createdDriver = await _repository.AddAsync(driver);
        await _repository.Save();
        
        var driverDto = _mapper.Map<Driver, DriverDTO>(createdDriver);
        return driverDto;
    }
}