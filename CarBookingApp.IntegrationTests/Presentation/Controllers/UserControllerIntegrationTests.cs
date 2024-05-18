using System.Net;
using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Infrastructure.Repositories;
using CarBookingApp.IntegrationTests.Helpers;
using CarBookingApp.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CarBookingApp.IntegrationTests.Presentation.Controllers;

public class UserControllerIntegrationTests
{
    [Fact]
    public async Task UserController_WhenUserDataIsPresent_ShouldGetUserById()
    {
        var userId = 1;

        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedUsers(userId);

        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new UserController(mediator);

        var resultRequest = await controller.GetUserById(userId);
        var result = resultRequest.Result as OkObjectResult;
        var user = result!.Value as UserDTO;

        Assert.Multiple(() =>
        {
            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
            Assert.Equal($"FirstName-{userId}", user.FirstName);
            Assert.Equal($"LastName-{userId}", user.LastName);
            Assert.Equal($"user{userId}@example.com", user.Email);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        });
    }

    [Fact]
    public async Task UserController_WhenDriverDataIsPresent_ShouldGetDriverById()
    {
        var driverId = 1;

        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedDrivers(driverId);

        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new UserController(mediator);

        var resultRequest = await controller.GetUserById(driverId);
        var result = resultRequest.Result as OkObjectResult;
        var driver = result!.Value as UserDTO;

        Assert.Multiple(() =>
        {
            Assert.NotNull(driver);
            Assert.Equal(driverId, driver.Id);
            Assert.Equal($"FirstName-{driverId}", driver.FirstName);
            Assert.Equal($"LastName-{driverId}", driver.LastName);
            Assert.Equal($"driver{driverId}@example.com", driver.Email);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        });
    }
    
    [Fact]
    public async Task UserController_WhenUsersArePresent_ShouldGetAllUsers()
    {
        var userCount = 3;

        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedUsers(userCount);

        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new UserController(mediator);

        var resultRequest = await controller.GetUsers();
        var result = resultRequest.Result as OkObjectResult;
        var users = result!.Value as List<UserDTO>;

        Assert.Multiple(() =>
        {
            Assert.NotNull(users);
            Assert.Equal(userCount, users.Count);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        });
    }
    
    [Fact]
    public async Task UserController_WhenUpdateCommandIsValid_ShouldUpdateUser()
    {
        var userCount = 1;

        using var contextBuilder = new DataContextBuilder();
        var dbContext = contextBuilder.GetContext();
        contextBuilder.SeedUsers(userCount);

        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new UserController(mediator);

        var updateUserCommand = new UpdateUserCommand
        {
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            Gender = "FEMALE",
            DateOfBirth = new DateTime(1985, 5, 15),
            Email = "updated.email@example.com",
            PhoneNumber = "555-0102",
            YearsOfExperience = 5
        };

        var resultRequest = await controller.UpdateUser(userCount, updateUserCommand);
        var result = resultRequest.Result as OkObjectResult;
        var user = result!.Value as UserDTO;

        Assert.Multiple(() =>
        {
            Assert.NotNull(user);
            Assert.Equal(userCount, user.Id);
            Assert.Equal("UpdatedFirstName", user.FirstName);
            Assert.Equal("UpdatedLastName", user.LastName);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        });
    }
    
    
    [Fact]
    public async Task UserController_WhenValidCommand_ShouldApproveUserForRide()
    {
        var numberOfUserRides = 1;
        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedUserRides(numberOfUserRides);
        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new UserController(mediator);

        var approveCommand = new ApproveUserForRideCommand
        {
            RideId = numberOfUserRides,
            PassengerId = numberOfUserRides
        };

        var resultRequest = await controller.ApproveUserForRide(approveCommand);
        var result = resultRequest.Result as OkObjectResult;
        var approvedRideId = (int)result!.Value;

        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(numberOfUserRides, approvedRideId);
    }
}