using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Application.Rides.Commands;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Rides.Commands;

public class BookRideCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BookRideCommandHandler _handler;

    public BookRideCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new BookRideCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Theory]
    [InlineData(1, 2)]
    public async Task BookRide_WithValidRequest_ShouldBookRide(int rideId, int passengerId)
    {
        var ride = new Ride
        {
            Id = rideId,
            TotalSeats = 2,
            Passengers = new List<User>(),
            DateOfTheRide = DateTime.Now,
            DestinationFrom = new Destination(){Name = "destinationFrom"},
            DestinationTo = new Destination(){Name = "destinationTo"},
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
        
        var rideShortInfoDTO = new RideShortInfoDTO
        {
            Id = rideId,
            TotalSeats = 2,
            DateOfTheRide = DateTime.Now,
            DestinationFrom = "destinationFrom",
            DestinationTo = "destinationTo"
        };
        var command = new BookRideCommand { RideId = rideId, PassengerId = passengerId };

        _mockRepository.Setup(repo => repo.GetByIdWithInclude(rideId,
                It.IsAny<Expression<Func<Ride, object>>[]>())).ReturnsAsync(ride);
         _mockMapper.Setup(mapper => mapper.Map<Ride, RideShortInfoDTO>(ride)).Returns(rideShortInfoDTO);

        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(rideShortInfoDTO.Id, ride.Id);
            Assert.IsType<RideShortInfoDTO>(result); 
        });
    }

    [Theory]
    [InlineData(1, 2)]
    public async Task BookRide_WhenNoAvailableSeats_ShouldThrowActionNotAllowedException(int rideId, int passengerId)
    {
        User passenger1 = null!;
        var passengers = new List<User>();
        passengers.Add(passenger1);
        var ride = new Ride
        {
            Id = rideId,
            TotalSeats = 1,
            Passengers = passengers,
            DateOfTheRide = DateTime.Now,
            DestinationFrom = null!,
            DestinationTo = null!,
            Owner = null!
        };
    
        _mockRepository.Setup(repo => repo.GetByIdWithInclude(rideId, 
                It.IsAny<Expression<Func<Ride, object>>[]>())).ReturnsAsync(ride);
    
        var command = new BookRideCommand { RideId = rideId, PassengerId = passengerId };
    
        await Assert.ThrowsAsync<ActionNotAllowedException>(() => _handler.Handle(command, CancellationToken.None));
    }
    
    [Theory]
    [InlineData(1, 2)]
    public async Task BookRide_WhenPassengerIsOwner_ShouldThrowActionNotAllowedException(int rideId, int passengerId)
    {
        var ride = new Ride
        {
            Id = rideId,
            TotalSeats = 2,
            Passengers = new List<User>(),
            DateOfTheRide = default,
            DestinationFrom = null!,
            DestinationTo = null!,
            Owner = new Driver
            {
                Id = passengerId,
                FirstName = null!,
                LastName = null!,
                Gender = Gender.FEMALE,
                DateOfBirth = default,
                Email = null!,
                PhoneNumber = null!
            },
        };

        _mockRepository.Setup(repo => repo.GetByIdWithInclude(rideId,
                It.IsAny<Expression<Func<Ride, object>>[]>())).ReturnsAsync(ride);
    
        var command = new BookRideCommand { RideId = rideId, PassengerId = passengerId };
    
        await Assert.ThrowsAsync<ActionNotAllowedException>(() => _handler.Handle(command, CancellationToken.None));
    }
}