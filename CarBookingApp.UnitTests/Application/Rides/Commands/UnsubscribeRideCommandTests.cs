using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Application.Rides.Commands;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Rides.Commands;

public class UnsubscribeRideCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UnsubscribeFromRideCommandHandler _handler;

    public UnsubscribeRideCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UnsubscribeFromRideCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }
    
    [Theory]
    [InlineData(1, 2)]
    public async Task UnsubscribeRide_WithValidCommand_Success(int rideId, int passengerId)
    {
        
        var userRide = new UserRide { RideStatus = RideStatus.UPCOMING, Id = 1 };

        var ride = new Ride
        {
            Id = rideId,
            DateOfTheRide = default,
            DestinationFrom = null!,
            DestinationTo = null!,
            TotalSeats = 1,
            Owner = new Driver
            {
                FirstName = null!,
                LastName = null!,
                Gender = Gender.FEMALE,
                DateOfBirth = default,
                Email = null!,
                PhoneNumber = null!
            }
        };

        _mockRepository.Setup(r => r.GetByPredicate(It.IsAny<Expression<Func<UserRide, bool>>>()))
            .ReturnsAsync([userRide]);
        _mockRepository.Setup(r => r.GetByIdWithInclude(rideId, It.IsAny<Expression<Func<Ride, object>>[]>()))
            .ReturnsAsync(ride);
        _mockRepository.Setup(r => r.DeleteAsync<UserRide>(userRide.Id))
            .ReturnsAsync(userRide);
        var command = new UnsubscribeFromRideCommand { RideId = rideId, PassengerId = passengerId };
        
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().NotThrowAsync<ActionNotAllowedException>();
    }
    
    [Theory]
    [InlineData(1, 2)]
    public async Task UnsubscribeRide_WhenRideIsOngoing_ShouldThrowActionNotAllowedException(int rideId, int passengerId)
    {
        
        var userRide = new UserRide { RideStatus = RideStatus.ONGOING };
        _mockRepository.Setup(r => r.GetByPredicate(It.IsAny<Expression<Func<UserRide, bool>>>()))
            .ReturnsAsync([userRide]);
        var command = new UnsubscribeFromRideCommand { RideId = rideId, PassengerId = passengerId };
        
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ActionNotAllowedException>();
    }
    
    [Theory]
    [InlineData(1, 2)]
    public async Task UnsubscribeRide_WhenPassengerIsOwner_ShouldThrowActionNotAllowedException(int rideId, int passengerId)
    {
        var userRide = new UserRide { RideStatus = RideStatus.UPCOMING, Id = 1 };

        var ride = new Ride
        {
            Id = rideId,
            DateOfTheRide = default,
            DestinationFrom = null!,
            DestinationTo = null!,
            TotalSeats = 1,
            Owner = new Driver
            {
                Id = passengerId,
                FirstName = null!,
                LastName = null!,
                Gender = Gender.FEMALE,
                DateOfBirth = default,
                Email = null!,
                PhoneNumber = null!
            }
        };

        _mockRepository.Setup(r => r.GetByPredicate(It.IsAny<Expression<Func<UserRide, bool>>>()))
            .ReturnsAsync([userRide]);
        _mockRepository.Setup(r => r.GetByIdWithInclude(rideId, 
                It.IsAny<Expression<Func<Ride, object>>[]>()))
            .ReturnsAsync(ride);

        var command = new UnsubscribeFromRideCommand { RideId = rideId, PassengerId = passengerId };
        
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        
        await act.Should().ThrowAsync<ActionNotAllowedException>();
    }
}