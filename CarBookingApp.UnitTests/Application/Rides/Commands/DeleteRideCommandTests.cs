using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Commands;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Rides.Commands;

public class DeleteRideCommandTests
{
    [Fact]
    public async Task Handle_ValidRideId_DeletesRideAndReturnsMappedDTO()
    {
        var mockRepository = new Mock<IRepository>();
        var mockMapper = new Mock<IMapper>();
        var handler = new DeleteRideCommandHandler(mockMapper.Object, mockRepository.Object);
        var rideId = 1;
        var ride = new Ride
        {
            Id = rideId,
            TotalSeats = 2,
            Passengers = new List<User>(),
            DateOfTheRide = default,
            DestinationFrom = null!,
            DestinationTo = null!,
            Owner = null!
        };
        var expectedDto = new RideShortInfoDTO { Id = rideId };

        mockRepository.Setup(repo => repo.DeleteAsyncWithInclude(rideId,
                It.IsAny<Expression<Func<Ride, object>>[]>()))
            .ReturnsAsync(ride);
        mockMapper.Setup(mapper => mapper.Map<Ride, RideShortInfoDTO>(ride))
            .Returns(expectedDto);
        var deleteCommand = new DeleteRideCommand(rideId);
        
        var result = await handler.Handle(deleteCommand, CancellationToken.None);
        
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(ride.Id, expectedDto.Id);
            Assert.IsType<RideShortInfoDTO>(result); 
        });
    }
}