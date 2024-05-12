using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Users.Commands;

public class DowngradeDriverToUserCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DowngradeDriverToUserCommandHandler _handler;

    public DowngradeDriverToUserCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new DowngradeDriverToUserCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task DowngradeDriverToUser_WithValidCommand_ShouldDowngradeDriverToUser()
    {
        var userId = 1;
        var command = new DowngradeDriverToUserCommand(userId);

        var driver = new Driver
        {
            Id = userId,
            FirstName = "Name",
            LastName = "LastName",
            Gender = Gender.MALE,
            DateOfBirth = new DateTime(1990, 10, 15),
            Email = null!,
            PhoneNumber = null!
        };

        var user = new User
        {
            Id = driver.Id,
            FirstName = driver.FirstName,
            LastName = driver.LastName,
            Gender = driver.Gender,
            DateOfBirth = driver.DateOfBirth,
            Email = driver.Email,
            PhoneNumber = driver.PhoneNumber
        };

        _mockRepository.Setup(repo => repo.GetByIdAsync<Driver>(command.Id))
                       .ReturnsAsync(driver);
        _mockMapper.Setup(mapper => mapper.Map<Driver, User>(driver))
                   .Returns(user);
        _mockRepository.Setup(repo => repo.DeleteAsync<Driver>(command.Id))
                       .ReturnsAsync(driver);
        _mockRepository.Setup(repo => repo.AddAsync(user))
                       .ReturnsAsync(user);
        _mockRepository.Setup(repo => repo.Save())
                       .Returns(Task.CompletedTask);
        _mockMapper.Setup(mapper => mapper.Map<User, UserDTO>(user))
                   .Returns(new UserDTO { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            _mockRepository.Verify(repo => repo.GetByIdAsync<Driver>(command.Id), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAsync<Driver>(command.Id), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync(user), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Exactly(2)); 
        });
    }
}