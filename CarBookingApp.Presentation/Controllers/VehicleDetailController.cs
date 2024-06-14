using CarBookingApp.Application.VehicleDetails.Commands;
using CarBookingApp.Application.VehicleDetails.Queries;
using CarBookingApp.Application.VehicleDetails.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class VehicleDetailController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleDetailController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("create/{userId}")]
    [Authorize(Roles ="User")]
    public async Task<ActionResult<VehicleDetailDTO>> CreateVehicleDetail(int userId, [FromBody] CreateVehicleDetailCommand createVehicleDetailCommand)
    {
        createVehicleDetailCommand.UserId = userId;
        var result = await _mediator.Send(createVehicleDetailCommand);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("info/update/{userId}")]
    [Authorize(Roles ="Driver")]
    public async Task<ActionResult<VehicleDetailDTO>> UpdateVehicleDetail(int userId, [FromBody] UpdateVehicleDetailCommand updateVehicleDetailCommand)
    {
        updateVehicleDetailCommand.UserId = userId;
        var result = await _mediator.Send(updateVehicleDetailCommand);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("info/{userId}")]
    [Authorize(Roles = "User, Driver")]

    public async Task<ActionResult<VehicleDetailDTO>> GetVehicleDetailById(int userId)
    {
        var result = await _mediator.Send(new GetVehicleDetailByIdQuery(userId));
        return Ok(result);
    }
}