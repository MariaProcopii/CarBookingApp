using System.Linq.Expressions;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Vehicles.Queries;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Vehicles.Queries;

public class GetAllModelsForVendorQueryTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly GetAllModelsForVendorQueryHandler _handler;

    public GetAllModelsForVendorQueryTests()
    {
        _mockRepository = new Mock<IRepository>();
        _handler = new GetAllModelsForVendorQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllModelsForVendor_WhenVendorExists_ShouldReturnListOfModels()
    {
        var vendor = "Toyota";
        var query = new GetAllModelsForVendorQuery { Vendor = vendor };
        var vehicles = new List<Vehicle>
        {
            new() { Vender = "Toyota", Model = "Corolla" },
            new() { Vender = "Toyota", Model = "Camry" },
            new() { Vender = "Toyota", Model = "Rav4" }
        };
        _mockRepository.Setup(repo => repo
            .GetByPredicate(It.IsAny<Expression<Func<Vehicle, bool>>>())).ReturnsAsync(vehicles);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(vehicles.Select(v => v.Model).Distinct(), result);
            _mockRepository.Verify(repo => repo
                .GetByPredicate(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
        });
    }

    [Fact]
    public async Task GetAllModelsForVendor_WhenVendorDoesNotExists_ShouldReturnEmptyList()
    {
        var vendor = "NonExistingVendor";
        var query = new GetAllModelsForVendorQuery { Vendor = vendor };
        var vehicles = new List<Vehicle>();
        _mockRepository.Setup(repo => repo
            .GetByPredicate(It.IsAny<Expression<Func<Vehicle, bool>>>())).ReturnsAsync(vehicles);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(repo => repo
                .GetByPredicate(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
        });
    }
}