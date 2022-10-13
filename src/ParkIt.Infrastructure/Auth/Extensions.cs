using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ParkIt.Application.Services;
using ParkIt.Infrastructure.DAL;
using PasswordGenerator;

namespace ParkIt.Infrastructure.Auth;

internal static class Extensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetRequiredSection("jwt"));
        var jwtOptions = new JwtOptions();
        var section = configuration.GetRequiredSection("jwt");
        section.Bind(jwtOptions);
        services.AddIdentityCore<ParkItUser>(x =>

        {
            x.Password.RequireDigit = false;
            x.Password.RequiredLength = 5;
            x.Password.RequireLowercase = false;
            x.Password.RequireUppercase = false;
            x.Password.RequireNonAlphanumeric = false;
        })
        .AddRoles<IdentityRole>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddEntityFrameworkStores<ParkItDbContext>();

        // services.AddScoped<IAuthHandler, AuthHandler>();
        services.AddHttpContextAccessor();
        services.AddSingleton<ITokenHandler, TokenHandler>();
        services.AddScoped<IUserService, UserService>();

        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.ClaimsIssuer = jwtOptions.Issuer;
                o.Audience = jwtOptions.Audience;
                o.IncludeErrorDetails = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
                };
            });

        services.AddAuthorization(authorization =>
        {
            authorization.AddPolicy("is-employee", policy =>
            {
                policy.RequireRole("User");
            });
            authorization.AddPolicy("is-admin", policy =>
            {
                policy.RequireRole("Admin");
            });
        });

        return services;
    }

    public static async Task SeedIdentity(IServiceProvider scopedServices)
    {
        var userManager = scopedServices.GetRequiredService<UserManager<ParkItUser>>();
        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();
        // var context = scopedServices.GetRequiredService<ParkItDbContext>();

        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var parkItUser = new ParkItUser
        {
            UserName = "admin@testing.com",
            EmployeeId = Guid.NewGuid(),
        };

        var generatedPassword = new Password();
        var password = generatedPassword.Next();
        var result = await userManager.CreateAsync(parkItUser, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(parkItUser, "Admin");
            await File.WriteAllTextAsync("password.txt", $"admin@testing.com, \npassword: '{password}'");
        }

        // var newEmployee = new Employee("Admin", "Admin@testing.com", DateTime.Now);
        // await context.AddAsync(newEmployee);
        // await context.SaveChangesAsync();
    }
}