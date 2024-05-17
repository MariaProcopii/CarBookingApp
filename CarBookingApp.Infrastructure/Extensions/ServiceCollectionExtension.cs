using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Auth;
using CarBookingApp.Infrastructure.Configurations;
using CarBookingApp.Infrastructure.Identity;
using CarBookingApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarBookingApp.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureServices(this IServiceCollection svcs, IConfiguration conf)
    {
        svcs
            .AddScoped<IRepository, Repository>()
            .AddDbContext<CarBookingAppDbContext>(cfg =>
                cfg.UseNpgsql(conf.GetConnectionString("DefaultDB")));
        
        svcs.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<IdentityRole<int>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddEntityFrameworkStores<CarBookingAppDbContext>();

        svcs.AddTransient<IJwtService, JwtService>();
    }
}