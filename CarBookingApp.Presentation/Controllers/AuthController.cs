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

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignInUser(SignInUserCommand signInUserCommand)
    {
        var accessToken = await _mediator.Send(signInUserCommand);
        return Ok(accessToken);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser(LogInUserCommand logInUserCommand)
    {
        var accessToken = await _mediator.Send(logInUserCommand);
        return Ok(accessToken);
    }
}