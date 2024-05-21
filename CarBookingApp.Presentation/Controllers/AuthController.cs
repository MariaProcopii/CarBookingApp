using CarBookingApp.Application.Auth.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUpUser(SignUpUserCommand signUpUserCommand)
    {
        var accessToken = await _mediator.Send(signUpUserCommand);
        return Ok(accessToken);
    }
    
    [HttpPost("LogIn")]
    public async Task<IActionResult> LogInUser(LogInUserCommand logInUserCommand)
    {
        var accessToken = await _mediator.Send(logInUserCommand);
        return Ok(accessToken);
    }
}