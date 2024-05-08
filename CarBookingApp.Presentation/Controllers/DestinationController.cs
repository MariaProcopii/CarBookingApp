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
    private readonly ILogger<UserController> _logger;

    private readonly IMediator _mediator;

    public DestinationController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
        
    [HttpGet]
    [Route("pick/name")]
    public async Task<List<DestinationDTO>> GetAllDestinations()
    {
        return await _mediator.Send(new GetAllDestinationsQuery());
    }
}