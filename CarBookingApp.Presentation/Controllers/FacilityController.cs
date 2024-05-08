using CarBookingApp.Application.Facilities.Queries;
using CarBookingApp.Application.Facilities.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;


[ApiController]
[Route("[controller]")]
public class FacilityController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly IMediator _mediator;

    public FacilityController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
        
    [HttpGet]
    [Route("pick/type")]
    public async Task<List<FacilityDTO>> GetAllFacilities()
    {
        return await _mediator.Send(new GetAllFacilitiesQuery());
    }
}