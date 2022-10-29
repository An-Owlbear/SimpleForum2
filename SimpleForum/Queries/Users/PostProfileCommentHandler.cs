using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Queries.Users;

public record PostProfileCommentRequest(string recipientId, string content) : IRequest<Result<PostProfileCommentResponse>>;

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
        if (String.IsNullOrWhiteSpace(request.content))
            return Result.Failure<PostProfileCommentResponse>("Comment cannot be blank", ErrorType.BadRequest);

        User? user = _currentUserAccessor.User;
        if (user == null) return Result.Failure<PostProfileCommentResponse>("Unauthorized", ErrorType.Unauthorized);

        User? recipient = await _context.Users.FindAsync(new object[] { request.recipientId }, cancellationToken);
        if (recipient == null) return Result.Failure<PostProfileCommentResponse>("Invalid user", ErrorType.NotFound);

        ProfileComment comment = new(request.content, user.Username, recipient.Username);
        await _context.ProfileComments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful(new PostProfileCommentResponse(comment.ProfileCommentId));
    }
}