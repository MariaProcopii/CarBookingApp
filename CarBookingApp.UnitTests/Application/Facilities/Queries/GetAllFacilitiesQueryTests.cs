using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Facilities.Queries;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Facilities.Queries;

public class GetAllFacilitiesQueryTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly GetAllFacilitiesQueryHandler _handler;
    public GetAllFacilitiesQueryTests()
    {
        _mockRepository = new Mock<IRepository>();
        _handler = new GetAllFacilitiesQueryHandler(_mockRepository.Object);
    }
    
    [Fact]
    public async Task GetAllFacilities_WithExistingData_ShouldReturnListOfFacilities()
    {
        var facilities = new List<Facility>
        {
            new Facility { Id = 1, FacilityType = "WiFi" },
            new Facility { Id = 2, FacilityType = "Air Conditioning" }
        };
        _mockRepository.Setup(repo => repo.GetAllAsync<Facility>()).ReturnsAsync(facilities);

        var result = await _handler.Handle(new GetAllFacilitiesQuery(), CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains("WiFi", result);
            Assert.Contains("Air Conditioning", result); 
        });
    }

    [Fact]
    public async Task GetAllFacilities_WithNoData_ShouldReturnEmptyList()
    {
        var mockRepository = new Mock<IRepository>();
        mockRepository.Setup(repo => repo.GetAllAsync<Facility>()).ReturnsAsync(new List<Facility>());
        var handler = new GetAllFacilitiesQueryHandler(mockRepository.Object);

        var result = await handler.Handle(new GetAllFacilitiesQuery(), CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Empty(result); 
        });
    }
}