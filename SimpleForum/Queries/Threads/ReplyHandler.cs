using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Queries.Threads;

public record ReplyRequest(string ThreadId, string Content) : IRequest<Result<ForumReply>>;

public class ReplyHandler : IRequestHandler<ReplyRequest, Result<ForumReply>>
{
    private readonly SimpleForumContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public ReplyHandler(SimpleForumContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<Result<ForumReply>> Handle(ReplyRequest param, CancellationToken cancellationToken = default)
    {
        if (_userAccessor.User == null) return Result.Failure<ForumReply>("User not signed in", ErrorType.Unauthorized);
        if (String.IsNullOrWhiteSpace(param.Content)) return Result.Failure<ForumReply>("Reply cannot be blank", ErrorType.BadRequest);

        ForumReply reply = new ForumReply(param.Content, param.ThreadId, _userAccessor.User.Username);
        await _context.Replies.AddAsync(reply, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful(reply);
    }
}