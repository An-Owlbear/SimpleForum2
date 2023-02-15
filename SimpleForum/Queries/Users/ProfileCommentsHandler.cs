using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Users;

/// <summary>
/// Retrieves a list of comments on the requested user's profile
/// </summary>
/// <param name="Id">The Id of the user to retrieve profile comments of</param>
public record ProfileCommentsRequest(string Id) : IRequest<Result<ProfileCommentsResponse>>;

/// <summary>
/// Contains user information, including comments on their profile
/// </summary>
/// <param name="User">The requested user</param>
public record ProfileCommentsResponse(User User);

public class ProfileCommentsHandler : IRequestHandler<ProfileCommentsRequest, Result<ProfileCommentsResponse>>
{
    private readonly SimpleForumContext _context;

    public ProfileCommentsHandler(SimpleForumContext context)
    {
        _context = context;
    }

    public async Task<Result<ProfileCommentsResponse>> Handle(ProfileCommentsRequest request, CancellationToken cancellationToken)
    {
        // Retrieves user, returning error if not found
        User? user = await _context.Users
            .Include(u => u.ReceivedProfileComments
                .OrderByDescending(p => p.DatePosted))
            .FirstOrDefaultAsync(u => u.Username == request.Id, cancellationToken);
            
        // Returns the user with comments if found, otherwise returns an error
        return user == null 
            ? Result.Failure<ProfileCommentsResponse>("User not found", ErrorType.NotFound) 
            : Result.Successful(new ProfileCommentsResponse(user));
    }
}