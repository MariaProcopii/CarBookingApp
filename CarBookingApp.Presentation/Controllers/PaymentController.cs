using CarBookingApp.Application.Payments.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "User, Driver")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand command)
    {
        var approvalUrl = await _mediator.Send(command);
        return Ok(new {approvalUrl});
    }
    
    [HttpPost]
    [Route("execute")]
    [Authorize(Roles = "User, Driver")]
    public async Task<IActionResult> ExecutePayment([FromBody] ExecutePaymentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}