using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CarBookingApp.Infrastructure.Options;

public class JwtSettings
{
    public string SigningKey { get; init; }
    public string Issuer { get; init; }
    public string[] Audiences { get; init; }
    public int TokenLifetime { get; init; }

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SigningKey));
    }
}