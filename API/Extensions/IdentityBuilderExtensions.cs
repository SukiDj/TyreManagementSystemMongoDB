using Microsoft.AspNetCore.Identity;

namespace API.Extensions;


public static class IdentityBuilderExtensions
{
    public static IdentityBuilder AddIdentityCookies(this IdentityBuilder builder)
    {
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/account/login";
            options.LogoutPath = "/account/logout";
            options.AccessDeniedPath = "/account/access-denied";
            options.SlidingExpiration = true;
        });

        return builder;
    }
}