using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Users.Commands;

public class UpdateUserCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateUserCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task UpdateUser_WhenSimpleUser_ShouldUpdateUser()
    {
        var userId = 1;
        var command = new UpdateUserCommand
        {
            Id = userId,
            FirstName = "Name",
            LastName = "LastName",
            Gender = "MALE",
            DateOfBirth = new DateTime(1990, 10, 15),
            Email = "new.email@example.com",
            PhoneNumber = "0987654321"
        };

        var existingUser = new User
        {
            Id = userId,
            FirstName = "ExistingFirstName",
            LastName = "ExistingLastName",
            Gender = Gender.FEMALE,
            DateOfBirth = new DateTime(1980, 5, 20),
            Email = "existing.email@example.com",
            PhoneNumber = "0987654321"
        };

        var updatedUser = new User
        {
            Id = command.Id,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Gender = Gender.MALE,
            DateOfBirth = command.DateOfBirth,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        };

        _mockRepository.Setup(repo => repo.GetByIdAsync<User>(command.Id))
            .ReturnsAsync(existingUser);
        _mockMapper.Setup(mapper => mapper.Map(command, existingUser))
            .Returns(updatedUser);
        _mockRepository.Setup(repo => repo.UpdateAsync(updatedUser))
            .ReturnsAsync(updatedUser);
        _mockRepository.Setup(repo => repo.Save())
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            _mockRepository.Verify(repo => repo.GetByIdAsync<User>(command.Id), Times.Once);
            _mockRepository.Verify(repo => repo.UpdateAsync(updatedUser), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
        });
    }
}