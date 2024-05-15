using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Vehicles.Queries;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Vehicles.Queries;

public class GetAllUniqueVendorQueryTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly GetAllUniqueVendorQueryHandler _handler;

    public GetAllUniqueVendorQueryTests()
    {
        _mockRepository = new Mock<IRepository>();
        _handler = new GetAllUniqueVendorQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllUniqueVendor_WhenRepositoryHasData_ReturnsListOfUniqueVendors()
    {
        var expectedVendors = new List<string> { "Toyota" };
        _mockRepository.Setup(repo => repo.GetAllAsync<Vehicle>())
            .ReturnsAsync(new List<Vehicle>
            {
                new()
                {
                    Vender = "Toyota",
                    Model = "Corolla"
                }
            });

        var result = await _handler.Handle(new GetAllUniqueVendorQuery(), CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(expectedVendors, result);
            _mockRepository.Verify(repo => repo.GetAllAsync<Vehicle>(), Times.Once); 
        });
    }

    [Fact]
    public async Task GetAllUniqueVendor_WhenRepositoryHasNoData_ReturnsEmptyList()
    {
        _mockRepository.Setup(repo => repo.GetAllAsync<Vehicle>())
            .ReturnsAsync(new List<Vehicle>());

        var result = await _handler.Handle(new GetAllUniqueVendorQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
        _mockRepository.Verify(repo => repo.GetAllAsync<Vehicle>(), Times.Once);
    }
}