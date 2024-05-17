using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarBookingApp.Application.Auth.Command;

public class LogInUserCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LogInUserCommandHandler : IRequestHandler<LogInUserCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;


    public LogInUserCommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    public async Task<string> Handle(LogInUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new EntityNotValidException("Invalid email");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            throw new EntityNotValidException("Invalid password");
        }

        var userClaims = await _userManager.GetClaimsAsync(user);

        var token = _jwtService.GenerateAccessToken(userClaims);

        return token;
    }
}