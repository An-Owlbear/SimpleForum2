using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SimpleForum.Models;

namespace SimpleForum.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    /// Signs in the given user, creating an authentication cookie
    /// </summary>
    /// <param name="httpContext">The httpcontext to authenticate them to</param>
    /// <param name="user">The user to authenticate</param>
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