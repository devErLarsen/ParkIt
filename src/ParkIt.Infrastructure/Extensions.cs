using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ParkIt.Infrastructure.Auth;
using ParkIt.Infrastructure.DAL;
// using ParkIt.Infrastructure.Middleware.Exceptions;

namespace ParkIt.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDAL(configuration)
            .AddAuth(configuration);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });

        });
        return services;
    }
    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        // init admin user.
        using var scope = app.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        Auth.Extensions.SeedIdentity(scopedServices).Wait();

        // app.UseMiddleware<GlobalErrorHandler>();

        app.UseSwagger();
        app.UseSwaggerUI();


        app.UseAuthentication();
        app.UseAuthorization();


        return app;
    }
}