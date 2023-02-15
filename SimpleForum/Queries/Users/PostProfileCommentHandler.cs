using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Queries.Users;

/// <summary>
/// Posts a comment to a user's profile
/// </summary>
/// <param name="recipientId">The Id of the profile to post to</param>
/// <param name="content">The content of the comment</param>
public record PostProfileCommentRequest(string recipientId, string content) : IRequest<Result<PostProfileCommentResponse>>;

/// <summary>
/// The Id of the newly created comment
/// </summary>
/// <param name="commentId">The Id of the newly created comment</param>
public record PostProfileCommentResponse(string commentId);

public class PostProfileCommentHandler : IRequestHandler<PostProfileCommentRequest, Result<PostProfileCommentResponse>>
{
    private readonly SimpleForumContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    
    public PostProfileCommentHandler(SimpleForumContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<PostProfileCommentResponse>> Handle(PostProfileCommentRequest request, CancellationToken cancellationToken)
    {
        // Ensures the submitted comment is not invalid
        if (String.IsNullOrWhiteSpace(request.content))
            return Result.Failure<PostProfileCommentResponse>("Comment cannot be blank", ErrorType.BadRequest);

        // Ensures the user has permissions to post the comment
        User? user = _currentUserAccessor.User;
        if (user == null) return Result.Failure<PostProfileCommentResponse>("Unauthorized", ErrorType.Unauthorized);

        // Ensures the specified recipient profile exists
        User? recipient = await _context.Users.FindAsync(new object[] { request.recipientId }, cancellationToken);
        if (recipient == null) return Result.Failure<PostProfileCommentResponse>("Invalid user", ErrorType.NotFound);

        // Creates the comment and saves it to the database
        ProfileComment comment = new(request.content, user.Username, recipient.Username);
        await _context.ProfileComments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful(new PostProfileCommentResponse(comment.ProfileCommentId));
    }
}