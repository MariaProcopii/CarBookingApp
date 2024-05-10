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
    private readonly ILogger<RideController> _logger;

    private readonly IMediator _mediator;

    public RideController(ILogger<RideController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("create/{ownerId}")]
    public async Task<RideWithRideDetailsInfoDTO> CreateRide(int ownerId, [FromBody] CreateRideCommand createRideCommand)
    {
        createRideCommand.OwnerId = ownerId;
        var result = await _mediator.Send(createRideCommand);
        return result;
    }
    
    [HttpDelete]
    [Route("delete/{rideId}")]
    public async Task<RideShortInfoDTO> DeleteRide(int rideId)
    {
        var result = await _mediator.Send(new DeleteRideCommand(rideId));
        return result;
    }
    
    [HttpPut]
    [Route("info/update/{rideId}")]
    public async Task<RideWithRideDetailsInfoDTO> UpdateRide(int rideId, [FromBody] UpdateRideCommand updateRideCommand)
    {
        updateRideCommand.RideId = rideId;
        return await _mediator.Send(updateRideCommand);
    }
    
    [HttpPost]
    [Route("info/book")]
    public async Task<RideShortInfoDTO> BookRide([FromBody] BookRideCommand bookRideCommand)
    {
        return await _mediator.Send(bookRideCommand);
    }
    
    [HttpPut]
    [Route("info/unsubscribe")]
    public async Task<RideShortInfoDTO> UnsubscribeFromRide([FromBody] UnsubscribeFromRideCommand unsubscribeFromRideCommand)
    {
        return await _mediator.Send(unsubscribeFromRideCommand);
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<List<RideShortInfoDTO>> GetAllRides(int userId)
    {
        return await _mediator.Send(new GetAllRidesQuery(userId));
    }
    
    [HttpGet]
    [Route("booked/{userId}")]
    public async Task<List<RideShortInfoDTO>> GetBookedRides(int userId)
    {
        return await _mediator.Send(new GetBookedRidesQuery(userId));
    }
    
    [HttpGet]
    [Route("pending/{userId}")]
    public async Task<List<RideShortInfoDTO>> GetPendingRides(int userId)
    {
        return await _mediator.Send(new GetPendingRidesQuery(userId));
    }
    
    [HttpGet]
    [Route("details/{rideId}")]
    public async Task<RideFullInfoDTO> GetRideInfoById(int rideId)
    {
        return await _mediator.Send(new GetRideInfoByIdQuery(rideId));
    }
}