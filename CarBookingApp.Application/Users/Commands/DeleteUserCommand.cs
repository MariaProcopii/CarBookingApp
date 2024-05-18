using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarBookingApp.Application.Users.Commands;

public record DeleteUserCommand(int UserId) : IRequest<int>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (appUser == null)
        {
            throw new EntityNotValidException("User not found.");
        }

        await _userManager.DeleteAsync(appUser!);
        
        return request.UserId;
    }
}