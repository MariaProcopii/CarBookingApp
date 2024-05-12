using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Queries;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Users.Queries;

public class GetAllUsersQueryTests
{
    private readonly Mock<IRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAllUsersQueryHandler _handler;

    public GetAllUsersQueryTests()
    {
        _mockRepository = new Mock<IRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetAllUsersQueryHandler(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllUsers_WhenListContainsSimpleUser_ShouldReturnListOfUsers()
    {
        var users = new List<User>();
        var user = new User
        {
            FirstName = null!,
            LastName = null!,
            Gender = Gender.MALE,
            DateOfBirth = new DateTime(1990, 10, 15),
            Email = null!,
            PhoneNumber = null!
        };
        users.Add(user);

        _mockRepository.Setup(repo => repo.GetAllAsync<User>())
            .ReturnsAsync(users);
        _mockMapper.Setup(mapper => mapper.Map<User, UserDTO>(user))
            .Returns(new UserDTO());

        await _handler.Handle(new GetAllUsersQuery(), CancellationToken.None);
        
        _mockRepository.Verify(repo => repo.GetAllAsync<User>(), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<User, UserDTO>(user), Times.Once);
    }
    
    [Fact]
    public async Task GetAllUsers_WhenListContainsDriver_ShouldReturnListOfDrivers()
    {
        var users = new List<User>();
        var user = new Driver
        {
            FirstName = null!,
            LastName = null!,
            Gender = Gender.MALE,
            DateOfBirth = new DateTime(1990, 10, 15),
            Email = null!,
            PhoneNumber = null!
        };
        users.Add(user);

        _mockRepository.Setup(repo => repo.GetAllAsync<User>())
            .ReturnsAsync(users);
        _mockMapper.Setup(mapper => mapper.Map<Driver, UserDTO>(user))
            .Returns(new UserDTO());

        await _handler.Handle(new GetAllUsersQuery(), CancellationToken.None);
        
        _mockRepository.Verify(repo => repo.GetAllAsync<User>(), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<Driver, UserDTO>(user), Times.Once);
    }
}