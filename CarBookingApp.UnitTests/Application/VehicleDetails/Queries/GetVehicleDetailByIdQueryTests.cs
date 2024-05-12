using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.VehicleDetails.Queries;
using CarBookingApp.Application.VehicleDetails.Responses;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.VehicleDetails.Queries;

public class GetVehicleDetailByIdQueryTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetVehicleDetailByIdQueryHandler _handler;

    public GetVehicleDetailByIdQueryTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetVehicleDetailByIdQueryHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ReturnsVehicleDetail_WhenFound()
    {
        var userId = 1;
        var vehicle = new List<Vehicle> { new() { Vender = "Toyota", Model = "Corolla" } };
        var vehicleDetail = new VehicleDetail
            { ManufactureYear = 2020, RegistrationNumber = "ABC123", Vehicle = vehicle.First(), Id = userId };
        _mockRepository.Setup(repo => repo.GetByIdWithInclude(userId,
            It.IsAny<Expression<Func<VehicleDetail, object>>>())).ReturnsAsync(vehicleDetail);
        _mockMapper.Setup(mapper => mapper.Map<VehicleDetail, VehicleDetailDTO>(vehicleDetail))
            .Returns(new VehicleDetailDTO());

        await _handler.Handle(new GetVehicleDetailByIdQuery(userId), CancellationToken.None);

        _mockRepository.Verify(repo => repo.GetByIdWithInclude(userId,
            It.IsAny<Expression<Func<VehicleDetail, object>>>()), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<VehicleDetail, VehicleDetailDTO>(vehicleDetail), Times.Once);
    }
}