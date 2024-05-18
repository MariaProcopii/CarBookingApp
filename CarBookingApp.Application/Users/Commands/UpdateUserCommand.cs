using System.Security.Claims;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Domain.Auth;
using CarBookingApp.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarBookingApp.Application.Users.Commands;

public class UpdateUserCommand : IRequest<string>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public int? YearsOfExperience { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;    
    private readonly IJwtService _jwtService;

    public UpdateUserCommandHandler(IRepository repository, IMapper mapper, UserManager<ApplicationUser> userManager, IJwtService jwtService)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {

        var existingUser = await _repository.GetByIdAsync<User>(request.Id);
        var appUser = await _userManager.FindByIdAsync(request.Id.ToString());
        if (request.Email != existingUser.Email)
        {
            var emailUser = await _userManager.FindByEmailAsync(request.Email);
            if (emailUser != null && emailUser.Id != appUser.Id)
            {
                throw new EntityNotValidException("Email already used.");
            }
            appUser.Email = request.Email;
            appUser.UserName = request.FirstName + request.LastName;
        }
        
        var updateResult = await _userManager.UpdateAsync(appUser);
        if (!updateResult.Succeeded)
        {
            throw new EntityNotValidException("Failed to update user.");
        }
        
        var userClaims = await _userManager.GetClaimsAsync(appUser);
        var roleClaims = userClaims.Where(c => c.Type == ClaimTypes.Role).ToList();
        await _userManager.RemoveClaimsAsync(appUser, userClaims);

        var newClaims = new List<Claim>
        {
            new(ClaimTypes.Name, request.FirstName),
            new(ClaimTypes.Surname, request.LastName),
            new(ClaimTypes.Email, request.Email)
        };
        newClaims.AddRange(roleClaims);
        await _userManager.AddClaimsAsync(appUser, newClaims);
        
        await _userManager.RemovePasswordAsync(appUser);
        await _userManager.AddPasswordAsync(appUser, request.Password);
        
        var updateUser = _mapper.Map(request, existingUser);
        if (existingUser is Driver existingDriver)
        { 
            updateUser = _mapper.Map(request, existingDriver);
        }
    
        await _repository.UpdateAsync(updateUser);
        await _repository.Save();

        var token = _jwtService.GenerateAccessToken(newClaims);
        return token;
    }
}