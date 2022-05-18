using System.Security.Claims;
using SimpleForum.Data;
using SimpleForum.Models;

namespace SimpleForum.Services;

public interface ICurrentUserAccessor
{
    public User? User { get; }
}

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

    public User? User
    {
        get
        {
            string username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _user ??= _context.Users.Find(username);
        }
    }
}