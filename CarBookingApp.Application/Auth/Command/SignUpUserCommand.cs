using System.Security.Claims;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Domain.Auth;
using CarBookingApp.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarBookingApp.Application.Auth.Command;

public class SignUpUserCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } 
}

public class SignInUserCommandHandler : IRequestHandler<SignUpUserCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public SignInUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, IJwtService jwtService, IRepository repository, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<string> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
    {
        var newAppUser = new ApplicationUser
        {
            UserName = request.FirstName + request.LastName,
            Email = request.Email
        };
        var isEmailPresent = await _userManager.FindByEmailAsync(request.Email);
        if (isEmailPresent is not null)
        {
            throw new EntityNotValidException("email: Email already used.");
        }
        var isPhonePresent = await _repository.GetByPredicate<User>(u => u.PhoneNumber
            .Equals(request.PhoneNumber));
        if (isPhonePresent.Count != 0)
        {
            Console.WriteLine();
            throw new EntityNotValidException("phone: Phone number already used.");
        }

        var result = await _userManager.CreateAsync(newAppUser, request.Password);
        var userId = await _userManager.GetUserIdAsync(newAppUser);
        var role = "User";

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
            new(ClaimTypes.Role, role),
            new(ClaimTypes.NameIdentifier, userId)
        };

        await _userManager.AddClaimsAsync(newAppUser, claims);
        var user = _mapper.Map<SignUpUserCommand, User>(request);
        user.Id = int.Parse(userId);
        await _repository.AddAsync(user);
        await _repository.Save();
        var token = _jwtService.GenerateAccessToken(claims);

        return await Task.FromResult(token);
    }
}