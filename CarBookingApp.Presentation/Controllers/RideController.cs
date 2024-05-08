using CarBookingApp.Application.Rides.Commands;
using CarBookingApp.Application.Rides.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class RideController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly IMediator _mediator;

    public RideController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("create/{userId}")]
    public async Task<RideDTO> CreateUser(int userId, [FromBody] CreateRideCommand createRideCommand)
    {
        createRideCommand.OwnerId = userId;
        var result = await _mediator.Send(createRideCommand);
        return result;
    }
}