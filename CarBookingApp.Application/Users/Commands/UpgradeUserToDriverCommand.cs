using System.Security.Claims;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Auth;
using CarBookingApp.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarBookingApp.Application.Users.Commands;

public class UpgradeUserToDriverCommand : IRequest<string>
{
    public int Id;
    public int YearsOfExperience { get; set; }
}

public class UpgradeToDriverCommandHandler : IRequestHandler<UpgradeUserToDriverCommand, string>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IJwtService _jwtService;


    public UpgradeToDriverCommandHandler(IRepository repository, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, IJwtService jwtService)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
    }

    public async Task<string> Handle(UpgradeUserToDriverCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync<User>(request.Id);
        var driver = _mapper.Map<User, Driver>(user);
        driver.YearsOfExperience = request.YearsOfExperience;
        
        await _repository.DeleteAsync<User>(request.Id);
        await _repository.AddAsync<User>(driver);
        await _repository.Save();
        
        var role = "Driver";
        var roleExists = await _roleManager.RoleExistsAsync(role);
        var appUser = await _userManager.FindByIdAsync(request.Id.ToString());

        if (!roleExists)
        {
            await _roleManager.CreateAsync(new IdentityRole<int>(role));
        }
                
        await _userManager.AddToRoleAsync(appUser!, role);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, role)
        };
        await _userManager.AddClaimsAsync(appUser!, claims);
        var userClaims = await _userManager.GetClaimsAsync(appUser!);

        var token = _jwtService.GenerateAccessToken(userClaims);
        return token;
    }
}