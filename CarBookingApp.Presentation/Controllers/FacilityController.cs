using CarBookingApp.Application.Facilities.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize]
public class FacilityController : ControllerBase
{
    private readonly IMediator _mediator;

    public FacilityController(IMediator mediator)
    {
        _mediator = mediator;
    }
        
    [HttpGet]
    [Route("pick/type")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<List<String>>> GetAllFacilities()
    {
        var result = await _mediator.Send(new GetAllFacilitiesQuery());
        return Ok(result);
    }
}