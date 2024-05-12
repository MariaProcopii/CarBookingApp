using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.VehicleDetails.Commands;
using CarBookingApp.Application.VehicleDetails.Responses;
using CarBookingApp.Application.Vehicles.Responses;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.VehicleDetails.Commands;

public class UpdateVehicleDetailCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateVehicleDetailCommandHandler _handler;

    public UpdateVehicleDetailCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateVehicleDetailCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task UpdatesVehicleDetail_ValidCommand_ShouldUpdatesVehicleDetail()
    {
        var userId = 1;
        var updateVehicleDetailCommand = new UpdateVehicleDetailCommand
        {
            UserId = userId,
            ManufactureYear = 2020,
            RegistrationNumber = "XYZ456",
            Vehicle = new VehicleDTO()
        };

        var vehicle = new List<Vehicle> { new() { Vender = "Toyota", Model = "Corolla" } };
        var vehicleDetailToUpdate = new VehicleDetail
            { ManufactureYear = 2010, RegistrationNumber = "ABC123", Vehicle = vehicle.First(), Id = userId };

        _mockRepository.Setup(repo => repo.GetByIdAsync<VehicleDetail>(userId))
            .ReturnsAsync(vehicleDetailToUpdate);
        _mockRepository.Setup(repo => repo.GetByPredicate(It.IsAny<Expression<Func<Vehicle, bool>>>()))
            .ReturnsAsync(vehicle);
        _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<VehicleDetail>()))
            .ReturnsAsync(vehicleDetailToUpdate);
        _mockMapper.Setup(mapper => mapper.Map<VehicleDetail, VehicleDetailDTO>(vehicleDetailToUpdate))
            .Returns(new VehicleDetailDTO());

        await _handler.Handle(updateVehicleDetailCommand, CancellationToken.None);
        
        _mockRepository.Verify(repo => repo.GetByIdAsync<VehicleDetail>(userId), Times.Once);
        _mockRepository.Verify(repo => repo.GetByPredicate(It.IsAny<Expression<Func<Vehicle, bool>>>()),
            Times.Once);
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<VehicleDetail>()), Times.Once);
    }
}