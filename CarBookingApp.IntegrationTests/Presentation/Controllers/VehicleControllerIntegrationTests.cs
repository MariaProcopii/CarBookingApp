using System.Net;
using CarBookingApp.Application.Vehicles.Queries;
using CarBookingApp.Infrastructure.Repositories;
using CarBookingApp.IntegrationTests.Helpers;
using CarBookingApp.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CarBookingApp.IntegrationTests.Presentation.Controllers;

public class VehicleControllerIntegrationTests
{
    [Fact]
    public async Task VehicleController_WhenDataIsPresent_ShouldGetAllVendorNames()
    {
        var vehicleNr = 3;
        
        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedVehicles(vehicleNr);
        
        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new VehicleController(mediator);

        var resultRequest = await controller.GetAllVendor();
        var result = resultRequest.Result as OkObjectResult;
        var vendors = result!.Value as List<string>;

        Assert.Multiple(() =>
        {
            Assert.NotNull(vendors);
            Assert.Equal(vehicleNr, vendors.Count);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode); 
        });
    }
    
    [Fact]
    public async Task VehicleController_WhenCommandIsValid_ShouldGetAllModelsForVendor()
    {
        var vehicleNr = 3;
        var query = new GetAllModelsForVendorQuery
        {
            Vendor = $"VendorName-{vehicleNr}"
        };
        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedVehicles(vehicleNr);
        
        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new VehicleController(mediator);

        var resultRequest = await controller.GetAllModelsForVendor(query);
        var result = resultRequest.Result as OkObjectResult;
        var vendors = result!.Value as List<string>;

        Assert.Multiple(() =>
        {
            Assert.NotNull(vendors);
            Assert.Single(vendors);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode); 
        });
    }
}