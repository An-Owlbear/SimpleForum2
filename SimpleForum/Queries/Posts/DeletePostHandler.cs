using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Queries.Posts;

public record DeletePostRequest(string Id) : IRequest<Result>;

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
        ForumReply? reply = await _context.Replies.FindAsync(new object[] { request.Id }, cancellationToken);
        if (reply == null) return Result.Failure("Post not found");
        if (reply.UserId != _userAccessor.User?.Username) return Result.Failure("Permission denied");
        reply.Content = "";
        reply.UserId = Constants.DeletedUser.Id;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful();
    }
}