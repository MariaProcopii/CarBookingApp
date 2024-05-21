using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Queries;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Rides.Queries;

public class GetAllRidesQueryTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllRidesQueryHandler _handler;
    public GetAllRidesQueryTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepository>();
        _handler = new GetAllRidesQueryHandler(_mockRepository.Object, _mockMapper.Object);
    }
    
    [Theory]
    [InlineData(1)]
    public async Task GetAllRides_WhenNoRides_ShouldReturnEmptyList(int userId)
    {
        var request = new GetAllRidesQuery(userId);

        var rides = new List<Ride>();
        _mockRepository.Setup(repo => repo.GetByPredicate(It.IsAny<Expression<Func<Ride, bool>>>(),
                It.IsAny<Expression<Func<Ride, object>>[]>()))
            .ReturnsAsync(rides);
        _mockMapper.Setup(mapper => mapper.Map<List<Ride>, List<RideShortInfoDTO>>(rides))
            .Returns(new List<RideShortInfoDTO>());

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        _mockRepository.Verify(x => x.GetByPredicate(It.IsAny<Expression<Func<Ride, bool>>>(),
            It.IsAny<Expression<Func<Ride, object>>[]>()), Times.Once());
        _mockMapper.Verify(x => x.Map<List<Ride>, List<RideShortInfoDTO>>(rides), Times.Once());
    }
}