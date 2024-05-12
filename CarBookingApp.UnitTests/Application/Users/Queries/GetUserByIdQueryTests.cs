using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Queries;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Moq;
using Xunit;

namespace CarBookingApp.UnitTests.Application.Users.Queries;

    public class GetUserByIdQueryTests
    {
        private readonly Mock<IRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryTests()
        {
            _mockRepository = new Mock<IRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetUserByIdQueryHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetUserById_WhenSimpleUser_ShouldReturnUser()
        {
            var userId = 1;
            var user = new User
            {
                Id = userId,
                FirstName = null!,
                LastName = null!,
                Gender = Gender.MALE,
                DateOfBirth = new DateTime(1990, 10, 15),
                Email = null!,
                PhoneNumber = null!
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync<User>(userId))
                           .ReturnsAsync(user);
            _mockMapper.Setup(mapper => mapper.Map<User, UserDTO>(user))
                       .Returns(new UserDTO());

            await _handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None);
            
            _mockRepository.Verify(repo => repo.GetByIdAsync<User>(userId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<User, UserDTO>(user), Times.Once);
        }

        [Fact]
        public async Task GetUserById_WhenUserIsDriver_ShouldReturnDriver()
        {
            var userId = 1;
            var user = new Driver
            {
                Id = userId,
                FirstName = null!,
                LastName = null!,
                Gender = Gender.MALE,
                DateOfBirth = new DateTime(1990, 10, 15),
                Email = null!,
                PhoneNumber = null!
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync<User>(userId))
                .ReturnsAsync(user);
            _mockMapper.Setup(mapper => mapper.Map<Driver, UserDTO>(user))
                .Returns(new UserDTO());

            await _handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None);
            
            _mockRepository.Verify(repo => repo.GetByIdAsync<User>(userId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<Driver, UserDTO>(user), Times.Once);
        }
    }