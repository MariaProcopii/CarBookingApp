using CarBookingApp.Application.Drivers.Commands;
using CarBookingApp.Application.Drivers.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class DriverController : ControllerBase
{
    private readonly ILogger<DriverController> _logger;

    private readonly IMediator _mediator;

    public DriverController(ILogger<DriverController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("create/{id}")]
    public async Task<DriverDTO> CreateUser(int id, [FromBody] UpgradeToDriverCommand upgradeToDriverCommand)
    {
        upgradeToDriverCommand.Id = id;
        var result = await _mediator.Send(upgradeToDriverCommand);
        return result;
    }
}