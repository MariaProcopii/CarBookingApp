using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Application.VehicleDetails.Commands;
using CarBookingApp.Application.VehicleDetails.Responses;
using CarBookingApp.Application.Vehicles.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.VehicleDetails.Commands;

public class CreateVehicleDetailCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateVehicleCommandHandler _handler;

    public CreateVehicleDetailCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateVehicleCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateVehicleDetailCommand_WhenUserIsDriver_ShouldCreateVehicleDetail()
    {
        var userId = 1;
        var vehicleDetailCommand = new CreateVehicleDetailCommand
        {
            UserId = userId,
            ManufactureYear = 2020,
            RegistrationNumber = "ABC123",
            Vehicle = new VehicleDTO()
        };

        var user = new Driver
        {
            Id = userId,
            FirstName = null!,
            LastName = null!,
            Gender = Gender.MALE,
            DateOfBirth = new DateTime(1990, 10, 15),
            Email = null!,
            PhoneNumber = null!
        };
        var vehicle = new List<Vehicle> { new() { Vender = "Toyota", Model = "Corolla" } };
        var vehicleDetail = new VehicleDetail
            { ManufactureYear = 2020, RegistrationNumber = "ABC123", Vehicle = vehicle.First(), Id = userId };

        _mockRepository.Setup(repo => repo.GetByIdAsync<User>(userId))
            .ReturnsAsync(user);
        _mockRepository.Setup(repo => repo.GetByPredicate(It
                    .IsAny<Expression<Func<Vehicle, bool>>>())).ReturnsAsync(vehicle);
        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<VehicleDetail>()))
            .ReturnsAsync(vehicleDetail);
        _mockMapper.Setup(mapper => mapper.Map<VehicleDetail, VehicleDetailDTO>(vehicleDetail))
            .Returns(new VehicleDetailDTO());

        await _handler.Handle(vehicleDetailCommand, CancellationToken.None);
        
        Assert.Multiple(() =>
        {
            _mockRepository.Verify(repo => repo.GetByIdAsync<User>(userId), Times.Once);
            _mockRepository.Verify(
                repo => repo.GetByPredicate(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<VehicleDetail>()), Times.Once);
        });
    }

    [Fact]
    public async Task CreateVehicleDetailCommand_WhenUserIsDriver_ThrowsActionNotAllowedException()
    {
        var userId = 1;
        var vehicleDetailCommand = new CreateVehicleDetailCommand
        {
            UserId = userId,
            ManufactureYear = 2020,
            RegistrationNumber = "ABC123",
            Vehicle = new VehicleDTO()
        };
    
        var user = new User
        {
            Id = userId,
            FirstName = null!,
            LastName = null!,
            Gender = Gender.MALE,
            DateOfBirth = new DateTime(1990, 10, 15),
            Email = null!,
            PhoneNumber = null!
        };
        _mockRepository.Setup(repo => repo.GetByIdAsync<User>(userId))
            .ReturnsAsync(user);
    
        await Assert.ThrowsAsync<ActionNotAllowedException>(
            () => _handler.Handle(vehicleDetailCommand, CancellationToken.None));

        _mockRepository.Verify(repo => repo.GetByIdAsync<User>(userId), Times.Once);
    }
}