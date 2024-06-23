using CarBookingApp.Application.Payments.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand command)
    {
        var approvalUrl = await _mediator.Send(command);
        return Ok(new {approvalUrl});
    }
    
    [HttpPost("execute")]
    public async Task<IActionResult> ExecutePayment([FromBody] ExecutePaymentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}