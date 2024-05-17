using System.Security.Claims;

namespace CarBookingApp.Application.Abstractions;

public interface IJwtService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);
}