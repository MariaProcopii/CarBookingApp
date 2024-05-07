using CarBookingApp.Application.Vehicles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly IMediator _mediator;

    public VehicleController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("pick/vendor")]
    public async Task<List<String>> GetAllVendor()
    {
        var result = await _mediator.Send(new GetAllUniqueVendorQuery());
        return result;
    }
    
    [HttpGet]
    [Route("pick/model")]
    public async Task<List<String>> GetAllModelsForVendor([FromBody] GetAllModelsForVendorQuery getAllModelsForVendorQuery)
    {
        var result = await _mediator.Send(getAllModelsForVendorQuery);
        return result;
    }
}