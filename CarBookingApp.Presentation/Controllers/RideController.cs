using CarBookingApp.Application.Common.Models;
using CarBookingApp.Application.Rides.Commands;
using CarBookingApp.Application.Rides.Queries;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Application.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RideController : ControllerBase
{
    private readonly IMediator _mediator;

    public RideController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("create/{ownerId}")]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<RideWithRideDetailsInfoDTO>> CreateRide(int ownerId, 
        [FromBody] CreateRideCommand createRideCommand)
    {
        createRideCommand.OwnerId = ownerId;
        var result = await _mediator.Send(createRideCommand);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("delete/{rideId}")]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<RideShortInfoDTO>> DeleteRide(int rideId)
    {
        var result = await _mediator.Send(new DeleteRideCommand(rideId));
        return Ok(result);
    }
    
    [HttpPut]
    [Route("complete/{rideId}")]
    [Authorize(Roles = "Driver")]
    public async Task<IActionResult> CompleteRide(int rideId)
    {
        await _mediator.Send(new CompleteRideCommand(rideId));
        return Ok();
    }
    
    [HttpPut]
    [Route("info/update/{rideId}")]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<RideWithRideDetailsInfoDTO>> UpdateRide(int rideId, 
        [FromBody] UpdateRideCommand updateRideCommand)
    {
        updateRideCommand.RideId = rideId;
        var result = await _mediator.Send(updateRideCommand);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("info/book")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<RideShortInfoDTO>> BookRide([FromBody] BookRideCommand bookRideCommand)
    {
        var result = await _mediator.Send(bookRideCommand);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("info/unsubscribe")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<RideShortInfoDTO>> UnsubscribeFromRide(
        [FromBody] UnsubscribeFromRideCommand unsubscribeFromRideCommand)
    {
        var result = await _mediator.Send(unsubscribeFromRideCommand);
        return Ok(result);
    }

    [HttpGet]
    [Route("{userId}")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<PaginatedList<RideShortInfoDTO>>> GetRides(
        int userId, 
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 8,
        [FromQuery] string orderBy = "DateOfTheRide",
        [FromQuery] bool ascending = true,
        [FromQuery] string destinationFrom = null,
        [FromQuery] string destinationTo = null,
        [FromQuery] DateTime? dateOfTheRide = null,
        [FromQuery] int totalSeats = 0)
    {
        var query = new GetAllRidesQuery(userId, pageNumber, pageSize, orderBy, ascending, 
                                        dateOfTheRide, destinationFrom, destinationTo, totalSeats);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("booked/{userId}")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<PaginatedList<RideShortInfoDTO>>> GetBookedRides(
        int userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 8,
        [FromQuery] string orderBy = "DateOfTheRide",
        [FromQuery] bool ascending = true)
    {
        var query = new GetBookedRidesQuery(userId, pageNumber, pageSize, orderBy, ascending);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("created/{userId}")]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<PaginatedList<RideCreatedInfoDTO>>> GetCreatedRides(
        int userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 8,
        [FromQuery] string orderBy = "DateOfTheRide",
        [FromQuery] bool ascending = true)
    {
        var query = new GetCreatedRidesQuery(userId, pageNumber, pageSize, orderBy, ascending);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("pending/{userId}")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<PaginatedList<RideShortInfoDTO>>> GetPendingRides(
        int userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 8,
        [FromQuery] string orderBy = "DateOfTheRide",
        [FromQuery] bool ascending = true)
    {
        var query = new GetPendingRidesQuery(userId, pageNumber, pageSize, orderBy, ascending);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("details/{rideId}")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<RideFullInfoDTO>> GetRideInfoById(int rideId)
    {
        var result = await _mediator.Send(new GetRideInfoByIdQuery(rideId));
        return Ok(result);
    }
    
    [HttpGet]
    [Route("completed/{userId}")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<List<UserDTO>>> GetInfoCompletedRides( int userId )
    {
        var query = new GetInfoCompletedRidesQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}