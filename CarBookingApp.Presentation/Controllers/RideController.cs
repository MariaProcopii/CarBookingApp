using CarBookingApp.Application.Rides.Commands;
using CarBookingApp.Application.Rides.Queries;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class RideController : ControllerBase
{
    private readonly IMediator _mediator;

    public RideController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("create/{ownerId}")]
    public async Task<ActionResult<RideWithRideDetailsInfoDTO>> CreateRide(int ownerId, 
        [FromBody] CreateRideCommand createRideCommand)
    {
        createRideCommand.OwnerId = ownerId;
        var result = await _mediator.Send(createRideCommand);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("delete/{rideId}")]
    public async Task<ActionResult<RideShortInfoDTO>> DeleteRide(int rideId)
    {
        var result = await _mediator.Send(new DeleteRideCommand(rideId));
        return Ok(result);
    }
    
    [HttpPut]
    [Route("info/update/{rideId}")]
    public async Task<ActionResult<RideWithRideDetailsInfoDTO>> UpdateRide(int rideId, 
        [FromBody] UpdateRideCommand updateRideCommand)
    {
        updateRideCommand.RideId = rideId;
        var result = await _mediator.Send(updateRideCommand);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("info/book")]
    public async Task<ActionResult<RideShortInfoDTO>> BookRide([FromBody] BookRideCommand bookRideCommand)
    {
        var result = await _mediator.Send(bookRideCommand);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("info/unsubscribe")]
    public async Task<ActionResult<RideShortInfoDTO>> UnsubscribeFromRide(
        [FromBody] UnsubscribeFromRideCommand unsubscribeFromRideCommand)
    {
        var result = await _mediator.Send(unsubscribeFromRideCommand);
        return Ok(result);
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<ActionResult<List<RideShortInfoDTO>>> GetAllRides(int userId)
    {
        var result = await _mediator.Send(new GetAllRidesQuery(userId));
        return Ok(result);
    }
    
    [HttpGet]
    [Route("booked/{userId}")]
    public async Task<ActionResult<List<RideShortInfoDTO>>> GetBookedRides(int userId)
    {
        var result = await _mediator.Send(new GetBookedRidesQuery(userId));
        return Ok(result);
    }
    
    [HttpGet]
    [Route("pending/{userId}")]
    public async Task<ActionResult<List<RideShortInfoDTO>>> GetPendingRides(int userId)
    {
        var result = await _mediator.Send(new GetPendingRidesQuery(userId));
        return Ok(result);
    }
    
    [HttpGet]
    [Route("details/{rideId}")]
    public async Task<ActionResult<RideFullInfoDTO>> GetRideInfoById(int rideId)
    {
        var result = await _mediator.Send(new GetRideInfoByIdQuery(rideId));
        return Ok(result);
    }
}