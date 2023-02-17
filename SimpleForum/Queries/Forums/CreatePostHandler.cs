using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Queries.Forums;

/// <summary>
/// Creates a new post
/// </summary>
/// <param name="Title">The title of the post</param>
/// <param name="Content">The body of the post</param>
/// <param name="ForumId">The id of the forum to post the thread to</param>
public record CreatePostRequest(string Title, string Content, string ForumId) : IRequest<Result<ForumThread>>;
    
public class CreatePostHandler : IRequestHandler<CreatePostRequest, Result<ForumThread>>
{
    private readonly SimpleForumContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public CreatePostHandler(SimpleForumContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<Result<ForumThread>> Handle(CreatePostRequest param, CancellationToken cancellationToken)
    {
        // Ensures the user is authenticated 
        if (_userAccessor.User == null) return Result.Failure<ForumThread>("User not signed in", ErrorType.Unauthorized);
        if (String.IsNullOrWhiteSpace(param.Title) || String.IsNullOrWhiteSpace(param.Content))
            return Result.Failure<ForumThread>("Title and content must cannot be empty", ErrorType.BadRequest);

        Forum? forum = await _context.Forums.FindAsync(new object[] { param.ForumId }, cancellationToken);
        if (forum == null) return Result.Failure<ForumThread>("Forum not found", ErrorType.NotFound);

        ForumThread thread = new(param.Title, _userAccessor.User.Username, forum.ForumId, param.Content);
        await _context.Threads.AddAsync(thread, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful(thread);
    }
}