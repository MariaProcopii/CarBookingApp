using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Vehicles.Queries;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Vehicles.Queries;

public class GetAllUniqueVendorQueryTests
{
    private readonly Mock<IVehicleRepository> _mockRepository;
    private readonly GetAllUniqueVendorQueryHandler _handler;

    public GetAllUniqueVendorQueryTests()
    {
        _mockRepository = new Mock<IVehicleRepository>();
        _handler = new GetAllUniqueVendorQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllUniqueVendor_WhenRepositoryHasData_ReturnsListOfUniqueVendors()
    {
        var expectedVendors = new List<string> { "Toyota", "Honda", "Ford" };

        _mockRepository.Setup(repo => repo.GetUniqueVendorListAsynk())
            .ReturnsAsync(expectedVendors);
        
        await _handler.Handle(new GetAllUniqueVendorQuery(), CancellationToken.None);
        
        _mockRepository.Verify(repo => repo.GetUniqueVendorListAsynk(), Times.Once);
    }

    [Fact]
    public async Task GetAllUniqueVendor_WhenRepositoryHasNoData_ReturnsEmptyList()
    {
        var expectedVendors = new List<string>();
        _mockRepository.Setup(repo => repo.GetUniqueVendorListAsynk())
            .ReturnsAsync(expectedVendors);

        var result = await _handler.Handle(new GetAllUniqueVendorQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
        _mockRepository.Verify(repo => repo.GetUniqueVendorListAsynk(), Times.Once);
    }
}