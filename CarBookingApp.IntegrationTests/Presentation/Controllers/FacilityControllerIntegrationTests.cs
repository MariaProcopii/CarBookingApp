using System.Net;
using CarBookingApp.Infrastructure.Repositories;
using CarBookingApp.IntegrationTests.Helpers;
using CarBookingApp.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CarBookingApp.IntegrationTests.Presentation.Controllers;

public class FacilityControllerIntegrationTests
{
    [Fact]
    public async Task FacilityController_WhenDataIsPresent_ShouldGetAllFacilities()
    {
        var facilityNr = 1;
        
        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedFacilities(facilityNr);
        
        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new FacilityController(mediator);

        var resultRequest = await controller.GetAllFacilities();
        var result = resultRequest.Result as OkObjectResult;
        var facilities = result!.Value as List<String>;

        Assert.Multiple(() =>
        {
            Assert.NotNull(facilities);
            Assert.Single(facilities);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode); 
        });
    }
}