using System.Security.Claims;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarBookingApp.Application.Auth.Command;

public class SignInUserCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } 
}

public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly IRepository _repository;
    public SignInUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, IJwtService jwtService, IRepository repository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _repository = repository;
    }

    public async Task<string> Handle(SignInUserCommand request, CancellationToken cancellationToken)
    {
        var newAppUser = new ApplicationUser
        {
            UserName = request.FirstName + request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
        
        var result = await _userManager.CreateAsync(newAppUser, request.Password);
        var role = "User";
        if (result.Succeeded)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
                
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(role));
            }
                
            await _userManager.AddToRoleAsync(newAppUser, role);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, request.FirstName),
                new(ClaimTypes.Surname, request.LastName),
                new(ClaimTypes.Email, request.Email),
                new(ClaimTypes.Role, role)
            };

            await _userManager.AddClaimsAsync(newAppUser, claims);

            var token = _jwtService.GenerateAccessToken(new List<Claim>
            {
                new(ClaimTypes.Name, request.FirstName),
                new(ClaimTypes.Surname, request.LastName),
                new(ClaimTypes.Email, request.Email),
                new(ClaimTypes.Role, role)
            });

            return await Task.FromResult(token);
        }
        else
        {
            throw new Exception("An error occured in the process of user registration");
        }
    }
}