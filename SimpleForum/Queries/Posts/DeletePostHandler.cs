using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Queries.Posts;

/// <summary>
/// Request for a user to delete their post, removing it's content and author information.
/// </summary>
/// <param name="Id">The ID of the post to delete</param>
public record DeletePostRequest(string Id) : IRequest<Result>;

/// <summary>
/// Allows a user to delete their own post from the forum. It leaves the entry in the database, but removes the post
/// content and sets the author to a placeholder 'deleted user'. This is done so that a 'deleted post' message can be
/// displayed.
/// </summary>
public class DeletePostHandler : IRequestHandler<DeletePostRequest, Result>
{
    private readonly SimpleForumContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public DeletePostHandler(SimpleForumContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }
    
    public async Task<Result> Handle(DeletePostRequest request, CancellationToken cancellationToken)
    {
        // Retrieves the post, and checks if it exists and the user is the creator of the post
        ForumReply? reply = await _context.Replies.FindAsync(new object[] { request.Id }, cancellationToken);
        if (reply == null) return Result.Failure("Post not found", ErrorType.NotFound);
        if (reply.UserId != _userAccessor.User?.Username) return Result.Failure("Permission denied", ErrorType.Forbidden);
        
        // Removes post content and user association, and saves changes
        reply.Content = "";
        reply.UserId = Constants.DeletedUser.Id;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful();
    }
}