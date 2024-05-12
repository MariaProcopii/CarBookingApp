using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Queries;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Rides.Queries;

public class GetPendingRidesQueryTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetPendingRidesQueryHandler _handler;
    public GetPendingRidesQueryTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepository>();
        _handler = new GetPendingRidesQueryHandler(_mockRepository.Object, _mockMapper.Object);
    }
    
    [Theory]
    [InlineData(1)]
    public async Task GetBookedRides_WhenNoBookedRides_ShouldReturnEmptyList(int userId)
    {
        var request = new GetPendingRidesQuery(userId);
        var rides = new List<Ride>();
        var userRides = new List<UserRide>();
        _mockRepository.Setup(repo => repo.GetByPredicate(It.IsAny<Expression<Func<UserRide, bool>>>(),
                It.IsAny<Expression<Func<UserRide, object>>[]>()))
            .ReturnsAsync(userRides);
        _mockMapper.Setup(mapper => mapper.Map<List<Ride>, List<RideShortInfoDTO>>(rides))
            .Returns(new List<RideShortInfoDTO>());

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
        _mockRepository.Verify(x => x.GetByPredicate(It.IsAny<Expression<Func<UserRide, bool>>>(),
            It.IsAny<Expression<Func<UserRide, object>>[]>()), Times.Once());
        _mockMapper.Verify(x => x.Map<List<Ride>, List<RideShortInfoDTO>>(rides), Times.Once());
    }
}