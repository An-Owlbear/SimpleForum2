using System.Security.Claims;
using SimpleForum.Data;
using SimpleForum.Models;

namespace SimpleForum.Services;

/// <summary>
/// Accesses information about the current user.
/// </summary>
public interface ICurrentUserAccessor
{
    public User? User { get; }
}

/// <inheritdoc />
public class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly SimpleForumContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(SimpleForumContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private User? _user;

    /// <summary>
    /// The current user. The first time this property is accessed each request will result in a database lookup for
    /// the user information.
    /// </summary>
    public User? User
    {
        get
        {
            string? username = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (username == null) return null;
            return _user ??= _context.Users.Find(username);
        }
    }
}