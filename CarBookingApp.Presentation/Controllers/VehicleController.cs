using CarBookingApp.Application.Vehicles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleController(IMediator mediator)
    { 
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("pick/vendor")]
    public async Task<ActionResult<List<String>>> GetAllVendor()
    {
        var result = await _mediator.Send(new GetAllUniqueVendorQuery());
        return Ok(result);
    }
    
    [HttpGet]
    [Route("pick/model")]
    public async Task<ActionResult<List<String>>> GetAllModelsForVendor([FromBody] GetAllModelsForVendorQuery getAllModelsForVendorQuery)
    {
        var result = await _mediator.Send(getAllModelsForVendorQuery);
        return Ok(result);
    }
}