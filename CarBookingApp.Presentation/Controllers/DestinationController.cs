using CarBookingApp.Application.Destinations.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class DestinationController : ControllerBase
{
    private readonly IMediator _mediator;

    public DestinationController(IMediator mediator)
    {
        _mediator = mediator;
    }
        
    [HttpGet]
    [Route("pick/name")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<List<String>>> GetAllDestinations()
    {
        var result = await _mediator.Send(new GetAllDestinationsQuery());
        return Ok(result);
    }
}