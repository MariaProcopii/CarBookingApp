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
using FluentAssertions;

namespace CarBookingApp.UnitTests.Application.Rides.Commands;

public class CreateRideCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateRideCommandHandler _handler;

    public CreateRideCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateRideCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }
    
    [Theory]
    [InlineData(1)]
    public async Task CreateRide_WithValidCommand_ShouldCreateRide(int ownerId)
    {
        var owner = new Driver
        {
            Id = ownerId,
            FirstName = null!,
            LastName = null!,
            Gender = Gender.FEMALE,
            DateOfBirth = default,
            Email = null!,
            PhoneNumber = null!
        };

        var destinations = new List<Destination> { new Destination() };

        var facility = new Facility { FacilityType = "facitily" };
        var facilities = new List<Facility> { facility };


        var command = new CreateRideCommand
        {
            DateOfTheRide = DateTime.Now,
            TotalSeats = 4,
            DestinationFrom = null!,
            DestinationTo = null!,
            OwnerId = ownerId,
            RideDetail = new RideDetailDTO()
        };

        _mockRepository.Setup(repo => repo.GetByIdAsync<User>(command.OwnerId)).ReturnsAsync(owner);
        _mockRepository.Setup(repo => repo.GetByPredicate(It.IsAny<Expression<Func<Destination, bool>>>()))
            .ReturnsAsync(destinations);
        _mockRepository.Setup(repo => repo.GetByPredicate(It.IsAny<Expression<Func<Destination, bool>>>()))
            .ReturnsAsync(destinations);
        _mockRepository.Setup(repo => repo.GetByPredicate(It.IsAny<Expression<Func<Facility, bool>>>()))
            .ReturnsAsync(facilities);
        
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().NotThrowAsync();
    }

    [Theory]
    [InlineData(1)]
    public async Task CreateRide_WhenOwnerIsNotDriver_ShouldThrowActionNotAllowedException(int ownerId)
    {
        var owner = new User
        {
            Id = ownerId,
            FirstName = null!,
            LastName = null!,
            Gender = Gender.FEMALE,
            DateOfBirth = default,
            Email = null!,
            PhoneNumber = null!
        };
        
        var command = new CreateRideCommand { OwnerId = ownerId };
        _mockRepository.Setup(repo => repo.GetByIdAsync<User>(ownerId)).ReturnsAsync(owner);
        
        await Assert.ThrowsAsync<ActionNotAllowedException>(() => _handler.Handle(command, CancellationToken.None));
    }
}