using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Users.Commands;

public class CreateUserCommandTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateUserCommandHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateUser()
    {
        var command = new CreateUserCommand
        {
            FirstName = "Name",
            LastName = "LastName",
            Gender = "MALE",
            DateOfBirth = new DateTime(1990, 10, 15),
            Email = null!,
            PhoneNumber = null!
        };

        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Gender = Gender.MALE,
            DateOfBirth = command.DateOfBirth,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        };

        _mockMapper.Setup(mapper => mapper.Map<CreateUserCommand, User>(command))
            .Returns(user);
        _mockRepository.Setup(repo => repo.AddAsync(user))
            .ReturnsAsync(user);
        _mockMapper.Setup(mapper => mapper.Map<User, UserDTO>(user))
            .Returns(new UserDTO { Id = 1, FirstName = user.FirstName, LastName = user.LastName });

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            _mockRepository.Verify(repo => repo.AddAsync(user), Times.Once);
            _mockRepository.Verify(repo => repo.Save(), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<User, UserDTO>(user), Times.Once);
        });
    }
}