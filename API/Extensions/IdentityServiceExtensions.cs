using System.Text;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]!));
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // (Optional) allow tokens via query for websockets/signalR routes
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireQualitySupervisorRole",
                policy => policy.RequireRole("QualitySupervisor"));
            options.AddPolicy("RequireBusinessUnitLeaderRole",
                policy => policy.RequireRole("BusinessUnitLeader"));
            options.AddPolicy("RequireProductionOperatorRole",
                policy => policy.RequireRole("ProductionOperator"));

            options.AddPolicy("RequireProductionOperatorRoleOrQualitySupervisorRole",
                policy => policy.RequireRole("ProductionOperator", "QualitySupervisor"));
            options.AddPolicy("RequireBusinessUnitLeaderRoleOrQualitySupervisorRole",
                policy => policy.RequireRole("BusinessUnitLeader", "QualitySupervisor"));
        });

        services.AddScoped<TokenService>();
        return services;
    }
}
