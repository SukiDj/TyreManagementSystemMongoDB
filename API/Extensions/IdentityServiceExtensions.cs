using Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    // public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    // {
    //     // Core Identity without EF stores; you'll plug in Mongo-backed stores later
    //     services.AddIdentityCore<User>(opt =>
    //     {
    //         opt.Password.RequireNonAlphanumeric = false;
    //         opt.User.RequireUniqueEmail = true;
    //     })
    //     .AddRoles<AppRole>()
    //     .AddRoleManager<RoleManager<AppRole>>()
    //     .AddSignInManager<SignInManager<User>>()    // still useful for cookie flows
    //     .AddDefaultTokenProviders();                // OK to keep (password reset, etc.), not JWT

    //     // Authentication: COOKIE ONLY (no JWT)
    //     services.AddAuthentication(options =>
    //     {
    //         options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    //         options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    //         options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    //     })
    //     .AddCookie(IdentityConstants.ApplicationScheme, options =>
    //     {
    //         options.LoginPath = "/account/login";
    //         options.LogoutPath = "/account/logout";
    //         options.AccessDeniedPath = "/account/access-denied";
    //         options.SlidingExpiration = true;
    //         // options.Cookie.Name = "tms.auth"; // optional
    //     });

    //     // Authorization policies (your controllers can keep using them)
    //     services.AddAuthorization(options =>
    //     {
    //         options.AddPolicy("RequireQualitySupervisorRole", policy =>
    //             policy.RequireRole("QualitySupervisor"));
    //         options.AddPolicy("RequireBusinessUnitLeaderRole", policy =>
    //             policy.RequireRole("BusinessUnitLeader"));
    //         options.AddPolicy("RequireProductionOperatorRole", policy =>
    //             policy.RequireRole("ProductionOperator"));

    //         options.AddPolicy("RequireProductionOperatorRoleOrQualitySupervisorRole", policy =>
    //             policy.RequireRole("ProductionOperator", "QualitySupervisor"));
    //         options.AddPolicy("RequireBusinessUnitLeaderRoleOrQualitySupervisorRole", policy =>
    //             policy.RequireRole("BusinessUnitLeader", "QualitySupervisor"));
    //     });

    //     // Note: NO TokenService, NO JWT
    //     // Next step will be adding Mongo-backed IUserStore/IUserRoleStore to persist users in Atlas.

    //     return services;
    // }
}
