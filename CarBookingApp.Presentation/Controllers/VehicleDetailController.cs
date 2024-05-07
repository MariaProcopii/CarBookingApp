using CarBookingApp.Application.VehicleDetails.Commands;
using CarBookingApp.Application.VehicleDetails.Queries;
using CarBookingApp.Application.VehicleDetails.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleDetailController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly IMediator _mediator;

    public VehicleDetailController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("create/{userId}")]
    public async Task<VehicleDetailDTO> CreateVehicleDetail(int userId, [FromBody] CreateVehicleDetailCommand createVehicleDetailCommand)
    {
        createVehicleDetailCommand.UserId = userId;
        var result = await _mediator.Send(createVehicleDetailCommand);
        return result;
    }
    
    [HttpPut]
    [Route("info/update/{userId}")]
    public async Task<VehicleDetailDTO> UpdateVehicleDetail(int userId, [FromBody] UpdateVehicleDetailCommand updateVehicleDetailCommand)
    {
        updateVehicleDetailCommand.UserId = userId;
        var result = await _mediator.Send(updateVehicleDetailCommand);
        return result;
    }
    
    [HttpGet]
    [Route("info/{userId}")]
    public async Task<VehicleDetailDTO> GetVehicleDetailById(int userId)
    {
        var result = await _mediator.Send(new GetVehicleDetailByIdQuery(userId));
        return result;
    }
}