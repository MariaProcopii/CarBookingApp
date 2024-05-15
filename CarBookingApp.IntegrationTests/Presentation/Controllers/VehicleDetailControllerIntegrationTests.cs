using System.Net;
using CarBookingApp.Application.VehicleDetails.Commands;
using CarBookingApp.Application.VehicleDetails.Responses;
using CarBookingApp.Application.Vehicles.Responses;
using CarBookingApp.Infrastructure.Repositories;
using CarBookingApp.IntegrationTests.Helpers;
using CarBookingApp.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CarBookingApp.IntegrationTests.Presentation.Controllers;

public class VehicleDetailControllerIntegrationTests
{
    [Fact]
    public async Task VehicleDetailController_WhenValidCommand_ShouldCreateVehicleDetail()
    {
        var nrOfElements = 1;
        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedVehicles(nrOfElements);
        contextBuilder.SeedDrivers(nrOfElements);
        
        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new VehicleDetailController(mediator);
        var command = new CreateVehicleDetailCommand
        {
            UserId = nrOfElements,
            ManufactureYear = 2020,
            RegistrationNumber = "AAA 123",
            Vehicle = new VehicleDTO
            {
                Model = $"VendorModel-{nrOfElements}",
                Vender = $"VendorName-{nrOfElements}"
            }
        };

        var resultRequest = await controller.CreateVehicleDetail(command.UserId, command);
        var result = resultRequest.Result as OkObjectResult;
        var vehicleDetail = result!.Value as VehicleDetailDTO;

        Assert.Multiple(() =>
        {
            Assert.NotNull(vehicleDetail);
            Assert.Equal(command.ManufactureYear, vehicleDetail.ManufactureYear);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode); 
        });
    }
    
    [Fact]
    public async Task VehicleDetailController_WhenValidCommand_ShouldUpdateVehicleDetail()
    {
        var nrOfElements = 1;
        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedVehicleDetails(nrOfElements);
        
        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new VehicleDetailController(mediator);
        var command = new UpdateVehicleDetailCommand
        {
            UserId = nrOfElements,
            ManufactureYear = 2020,
            RegistrationNumber = "AAA 123",
            Vehicle = new VehicleDTO
            {
                Model = $"VendorModel-{nrOfElements}",
                Vender = $"VendorName-{nrOfElements}"
            }
        };

        var resultRequest = await controller.UpdateVehicleDetail(command.UserId, command);
        var result = resultRequest.Result as OkObjectResult;
        var vehicleDetail = result!.Value as VehicleDetailDTO;

        Assert.Multiple(() =>
        {
            Assert.NotNull(vehicleDetail);
            Assert.Equal(command.ManufactureYear, vehicleDetail.ManufactureYear);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode); 
        });
    }
    
    [Fact]
    public async Task VehicleDetailController_WhenValidCommand_ShouldGetVehicleDetailById()
    {
        var nrOfElements = 1;
        using var contextBuilder = new DataContextBuilder();
        contextBuilder.SeedVehicleDetails(nrOfElements);
        
        var dbContext = contextBuilder.GetContext();
        var mediator = TestHelpers.CreateMediator(new Repository(dbContext));
        var controller = new VehicleDetailController(mediator);

        var resultRequest = await controller.GetVehicleDetailById(nrOfElements);
        var result = resultRequest.Result as OkObjectResult;
        var vehicleDetail = result!.Value as VehicleDetailDTO;

        Assert.Multiple(() =>
        {
            Assert.NotNull(vehicleDetail);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode); 
        });
    }
}