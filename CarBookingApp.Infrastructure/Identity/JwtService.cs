using System.Security.Claims;
using CarBookingApp.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using CarBookingApp.Application.Abstractions;

namespace CarBookingApp.Infrastructure.Identity;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var signinCredentials = new SigningCredentials(_jwtSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audiences[0],
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.TokenLifetime),
            signingCredentials: signinCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        
        return tokenHandler.WriteToken(jwtSecurityToken);
    }
}