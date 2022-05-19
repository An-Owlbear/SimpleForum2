using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SimpleForum.Models;

namespace SimpleForum.Extensions;

public static class HttpContextExtensions
{
    public static async Task SignInAsync(this HttpContext httpContext, User user)
    {
        // Sets authentication cookie
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username)
        };
        ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));
    }
}