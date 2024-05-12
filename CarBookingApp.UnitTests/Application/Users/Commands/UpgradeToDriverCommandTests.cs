using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Users.Commands;

public class UpgradeToDriverCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpgradeToDriverCommandHandler _handler;

    public UpgradeToDriverCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpgradeToDriverCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task UpgradeUserToDriver_WithValidCommand_ShouldUpgradeUserToDriver()
    {
        var userId = 1;
        var command = new UpgradeUserToDriverCommand
        {
            Id = userId,
            YearsOfExperience = 5
        };

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

        _mockRepository.Setup(repo => repo.GetByIdAsync<User>(command.Id))
                       .ReturnsAsync(user);
        _mockMapper.Setup(mapper => mapper.Map<User, Driver>(user))
                   .Returns(driver);
        _mockRepository.Setup(repo => repo.DeleteAsync<User>(command.Id))
                       .ReturnsAsync(user);
        _mockRepository.Setup(repo => repo.AddAsync<User>(driver))
                       .ReturnsAsync(driver);
        _mockRepository.Setup(repo => repo.Save())
                       .Returns(Task.CompletedTask);
        _mockMapper.Setup(mapper => mapper.Map<User, UserDTO>(driver))
                   .Returns(new UserDTO { Id = driver.Id, FirstName = driver.FirstName, LastName = driver.LastName });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(driver.Id, result.Id);
            _mockRepository.Verify(repo => repo.GetByIdAsync<User>(command.Id), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAsync<User>(command.Id), Times.Once);
            _mockRepository.Verify(repo => repo.AddAsync<User>(driver), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once); 
        });
    }
}