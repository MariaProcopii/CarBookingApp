using System.Linq.Expressions;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Users.Commands;

public class ApproveUserForRideCommandHandlerTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly ApproveUserForRideCommandHandler _handler;

    public ApproveUserForRideCommandHandlerTests()
    {
        _mockRepository = new Mock<IRepository>();
        _handler = new ApproveUserForRideCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateRide_WithAvailableUserToApprove_ShouldCreateRide()
    {
        var command = new ApproveUserForRideCommand
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

        _mockRepository.Setup(repo => repo.GetByPredicate(It.IsAny<Expression<Func<UserRide, bool>>>()))
                       .ReturnsAsync([userRide]);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(BookingStatus.APPROVED, userRide.BookingStatus);
       _mockRepository.Verify(repo => repo.Save(), Times.Once);
        Assert.Equal(command.RideId, result);
    }
    
    [Fact]
    public async Task CreateRide_WithNoUserToApprove_ShouldThrowActionNotAllowedException()
    {
        var command = new ApproveUserForRideCommand
        {
            RideId = 1,
            PassengerId = 1
        };
    
        _mockRepository.Setup(repo => repo.GetByPredicate(It.IsAny<Expression<Func<UserRide, bool>>>()))
                       .ReturnsAsync([]);
    
        await Assert.ThrowsAsync<ActionNotAllowedException>(() => _handler.Handle(command, CancellationToken.None));
    }
}