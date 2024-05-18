using System.Security.Claims;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Auth;
using CarBookingApp.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarBookingApp.Application.Users.Commands;

public record DowngradeDriverToUserCommand(int Id) : IRequest<string>;

public class DowngradeDriverToUserCommandHandler : IRequestHandler<DowngradeDriverToUserCommand, string>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IJwtService _jwtService;

    public DowngradeDriverToUserCommandHandler(IRepository repository, IMapper mapper, 
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, IJwtService jwtService)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
    }

    public async Task<string> Handle(DowngradeDriverToUserCommand request, CancellationToken cancellationToken)
    {
        var driver = await _repository.GetByIdAsync<Driver>(request.Id);
        var user = _mapper.Map<Driver, User>(driver);

        await _repository.DeleteAsync<Driver>(request.Id);
        await _repository.AddAsync(user);
        await _repository.Save();
        
        var appUser = await _userManager.FindByIdAsync(request.Id.ToString());
        var role = "Driver";

        if (await _roleManager.RoleExistsAsync(role))
        {
            await _userManager.RemoveFromRoleAsync(appUser!, role);
        }
        
        var userClaims = await _userManager.GetClaimsAsync(appUser!);
        var claimsToRemove = userClaims.Where(c => c.Type == ClaimTypes.Role && c.Value == role);
        await _userManager.RemoveClaimsAsync(appUser!, claimsToRemove);

        var updatedUserClaims = await _userManager.GetClaimsAsync(appUser!);
        var token = _jwtService.GenerateAccessToken(updatedUserClaims);

        return token;
    }
}