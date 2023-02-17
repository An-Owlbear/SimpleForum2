using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Queries.Users;

/// <summary>
/// Allows the user to delete a comment they made an another user's profile
/// </summary>
/// <param name="id">The id of the comment to delete</param>
public record DeleteProfileCommentRequest(string id) : IRequest<Result>;

public class DeleteProfileCommentHandler : IRequestHandler<DeleteProfileCommentRequest, Result>
{
    private readonly SimpleForumContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public DeleteProfileCommentHandler(SimpleForumContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result> Handle(DeleteProfileCommentRequest request, CancellationToken cancellationToken)
    {
        // Checks whether the comment exists, returning failure otherwise
        ProfileComment? comment =
            await _context.ProfileComments.FindAsync(new object[] { request.id }, cancellationToken);
        if (comment == null)
            return Result.Failure<DeleteProfileCommentRequest>("Comment not found", ErrorType.NotFound);

        // Checks user is the owner of the comment
        if (comment.UserId != _currentUserAccessor.User?.Username)
            return Result.Failure<DeleteProfileCommentRequest>("Forbidden", ErrorType.Forbidden);

        // Deletes comment and saves changes
        _context.ProfileComments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful();
    }
}