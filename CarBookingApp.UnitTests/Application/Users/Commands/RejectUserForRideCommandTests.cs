using System.Linq.Expressions;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Users.Commands;

public class RejectUserForRideCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly RejectUserForRideCommandHandler _handler;

    public RejectUserForRideCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _handler = new RejectUserForRideCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task RejectUserForRide_WithValidCommand_ShouldReturnUserRideRejectedId()
    {
        var command = new RejectUserForRideCommand
        {
            RideId = 1,
            PassengerId = 1
        };

        var userRide = new UserRide
        {
            PassengerId = command.PassengerId,
            RideId = command.RideId,
            BookingStatus = BookingStatus.PENDING
        };

        _mockRepository.Setup(repo => repo.GetByPredicate(
                It.IsAny<Expression<Func<UserRide, bool>>>())).ReturnsAsync([userRide]);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(BookingStatus.REJECTED, userRide.BookingStatus);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
        Assert.Equal(command.RideId, result);
    }

    [Fact]
    public async Task RejectUserForRide_UserAlreadyApproved_ThrowsActionNotAllowedException()
    {
        var command = new RejectUserForRideCommand
        {
            RideId = 1,
            PassengerId = 1
        };

        var userRide = new UserRide
        {
            PassengerId = command.PassengerId,
            RideId = command.RideId,
            BookingStatus = BookingStatus.APPROVED
        };

        _mockRepository.Setup(repo => repo.GetByPredicate(
                It.IsAny<Expression<Func<UserRide, bool>>>())).ReturnsAsync([userRide]);

        await Assert.ThrowsAsync<ActionNotAllowedException>(() => _handler.Handle(command, CancellationToken.None));
    }
}