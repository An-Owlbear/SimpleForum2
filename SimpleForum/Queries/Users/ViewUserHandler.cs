using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Users;

public record ViewUserRequest(string UserId) : IRequest<Result<ViewUserResponse>>;

public record ViewUserResponse
{
    public string Username { get; } = default!;
    public string Bio { get; } = default!;
    public string ProfileImage { get; } = default!;
    public DateTime DateJoined { get; }
    public IEnumerable<ForumThread> RecentThreads { get; } = default!;
    public IEnumerable<ForumReply> RecentReplies { get; } = default!;
    public IEnumerable<ProfileComment> ProfileComments { get; } = default!;

    public ViewUserResponse(User user)
    {
        Username = user.Username;
        Bio = user.Bio;
        ProfileImage = user.ProfileImage;
        DateJoined = user.DateJoined;
        RecentThreads = user.Threads;
        RecentReplies = user.Replies;
        ProfileComments = user.ReceivedProfileComments;
    }
}

public class ViewUserHandler : IRequestHandler<ViewUserRequest, Result<ViewUserResponse>>
{
    private readonly SimpleForumContext _context;


    public ViewUserHandler(SimpleForumContext context)
    {
        _context = context;
    }

    public async Task<Result<ViewUserResponse>> Handle(ViewUserRequest request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users
            .Include(u => u.Threads
                .OrderByDescending(t => t.DatePosted)
                .Take(5))
            .ThenInclude(t => t.Forum)
            .Include(u => u.Replies
                .OrderByDescending(r => r.DatePosted)
                .Take(5))
            .ThenInclude(r => r.Thread)
            .ThenInclude(t => t.Forum)
            .Include(u => u.ReceivedProfileComments
                .OrderByDescending(p => p.DatePosted)
                .Take(5))
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(u => u.Username == request.UserId, cancellationToken);

        return user == null
            ? Result.Failure<ViewUserResponse>("User not found", ErrorType.NotFound)
            : Result.Successful(new ViewUserResponse(user));
    }
}