using System.Net;
using CarBookingApp.Infrastructure.Repositories;
using CarBookingApp.IntegrationTests.Helpers;
using CarBookingApp.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CarBookingApp.IntegrationTests.Presentation.Controllers;

public class DestinationControllerIntegrationTests
{
    [Fact]
    public async Task DestinationController_WhenDataIsPresent_ShouldGetAllDestinations()
    {
        var destivationNr = 1;
        
        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedDestinations(destivationNr);
        
        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new DestinationController(mediator);
        
        var resultRequest = await controller.GetAllDestinations();
        var result = resultRequest.Result as OkObjectResult;
        var destinations = result!.Value as List<string>;

        Assert.Multiple(() =>
        {
            Assert.NotNull(destinations);
            Assert.Single(destinations);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode); 
        });
    }
}