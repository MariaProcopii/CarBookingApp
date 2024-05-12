using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Destinations.Queries;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Destinations.Queries;

public class GetAllDestinationsQueryTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly GetAllDestinationsQueryHandler _handler;
    public GetAllDestinationsQueryTests()
    {
        _mockRepository = new Mock<IRepository>();
        _handler = new GetAllDestinationsQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllDestinations_WithExistingData_ShouldReturnListOfDestinations()
    {
        var destinations = new List<Destination>
        {
            new Destination { Id = 1, Name = "Destination 1" },
            new Destination { Id = 2, Name = "Destination 2" }
        };
        _mockRepository.Setup(repo => repo.GetAllAsync<Destination>()).ReturnsAsync(destinations);

        var result = await _handler.Handle(new GetAllDestinationsQuery(), CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains("Destination 1", result);
            Assert.Contains("Destination 2", result);
        });
    }
    
    [Fact]
    public async Task GetAllDestinations_WithNoData_ShouldReturnEmptyList()
    {
        _mockRepository.Setup(repo => repo.GetAllAsync<Destination>()).ReturnsAsync(new List<Destination>());

        var result = await _handler.Handle(new GetAllDestinationsQuery(), CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Empty(result); 
        });
    }
}