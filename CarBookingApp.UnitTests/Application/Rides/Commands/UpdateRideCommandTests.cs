using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Commands;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Rides.Commands;

public class UpdateRideCommandHandlerTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateRideCommandHandler _handler;

    public UpdateRideCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateRideCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Theory]
    [InlineData(1)]
    public async Task UpdateRide_WhenValidCommand_Success(int rideId)
    {
        var request = new UpdateRideCommand
        {
            RideId = rideId,
            DateOfTheRide = DateTime.Now,
            DestinationFrom = null!,
            DestinationTo = null!,
            TotalSeats = 0,
            RideDetail = new RideDetailDTO
            {
                PickUpSpot = null!,
                Price = 0,
                Facilities = ["facility1", "facility2"]
            }
        };

        var rideToUpdate = new Ride
        {
            Id = rideId,
            DateOfTheRide = DateTime.Now,
            DestinationFrom = null!,
            DestinationTo = null!,
            TotalSeats = 0,
            Owner = null!,
            RideDetail = new RideDetail
            {
                PickUpSpot = null!,
                Price = 0
            }
        };

        var destinations = new List<Destination> { new Destination() };

        var facility = new Facility { FacilityType = "facitily" };
        var facilities = new List<Facility> { facility };

        _mockRepository.Setup(repo => repo.GetByIdWithInclude(rideId,
                It.IsAny<Expression<Func<Ride, object>>[]>()))
            .ReturnsAsync(rideToUpdate);
        _mockRepository.Setup(repo => repo.GetByPredicate(It.IsAny<Expression<Func<Destination, bool>>>()))
            .ReturnsAsync(destinations);
        _mockRepository.Setup(repo => repo.GetByPredicate(It.IsAny<Expression<Func<Facility, bool>>>()))
            .ReturnsAsync(facilities);
        _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Ride>())).ReturnsAsync(rideToUpdate);

        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }
}