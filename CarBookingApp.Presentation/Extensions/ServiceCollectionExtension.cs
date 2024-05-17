using System.Text;
using CarBookingApp.Application.Extensions;
using CarBookingApp.Infrastructure.Extensions;
using CarBookingApp.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CarBookingApp.Presentation.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPresentationServices(this IServiceCollection svcs, IConfiguration conf)
    {
        svcs.AddInfrastructureServices(conf);
        svcs.AddApplicationServices();
        svcs.AddControllers();
        svcs.AddEndpointsApiExplorer();
        svcs.AddAuthToSwagger();
        
        var jwtSettings = new JwtSettings();
        conf.Bind(nameof(JwtSettings), jwtSettings);

        var jwtSection = conf.GetSection(nameof(JwtSettings));
        svcs.Configure<JwtSettings>(jwtSection);
        
        svcs.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidAudiences = jwtSettings.Audiences,
                ValidIssuer = jwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey))
            };
            options.ClaimsIssuer = jwtSettings.Issuer;
        });
        
    }
    
    public static IServiceCollection AddAuthToSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter a valid Jwt",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}