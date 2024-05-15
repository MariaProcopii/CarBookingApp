using CarBookingApp.Application.Destinations.Queries;
using CarBookingApp.Application.Destinations.Responses;
using CarBookingApp.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class DestinationController : ControllerBase
{
    private readonly IMediator _mediator;

    public DestinationController(IMediator mediator)
    {
        _mediator = mediator;
    }
        
    [HttpGet]
    [Route("pick/name")]
    public async Task<ActionResult<List<String>>> GetAllDestinations()
    {
        var result = await _mediator.Send(new GetAllDestinationsQuery());
        return Ok(result);
    }
}