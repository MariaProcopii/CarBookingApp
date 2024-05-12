using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using CarBookingApp.Infrastructure.Exceptions;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Users.Commands;

public class DeleteUserCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new DeleteUserCommandHandler(_mockMapper.Object, _mockRepository.Object);
    }

    [Fact]
    public async Task DeleteUser_WithValidUserId_ShouldDeleteUser()
    {
        var userId = 1;
        var command = new DeleteUserCommand(userId);

        var user = new User
        {
            Id = userId,
            FirstName = "Name",
            LastName = "LastName",
            Gender = Gender.MALE,
            DateOfBirth = new DateTime(1990, 10, 15),
            Email = null!,
            PhoneNumber = null!
        };

        _mockRepository.Setup(repo => repo.DeleteAsync<User>(command.UserId))
            .ReturnsAsync(user);

        _mockMapper.Setup(mapper => mapper.Map<User, UserDTO>(user))
            .Returns(new UserDTO { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        _mockRepository.Verify(repo => repo.DeleteAsync<User>(command.UserId), Times.Once);
        _mockRepository.Verify(repo => repo.Save(), Times.Once);
    }
    
    [Fact]
    public async Task DeleteUser_WithValidUserId_ShouldThrowEntityNotFoundException()
    {
        var invalidUserId = Int16.MaxValue;
        var command = new DeleteUserCommand(invalidUserId);

        _mockRepository.Setup(repo => repo.DeleteAsync<User>(command.UserId))
            .ThrowsAsync(new EntityNotFoundException(""));
        
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}