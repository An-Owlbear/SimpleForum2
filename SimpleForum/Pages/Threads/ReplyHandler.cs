using SimpleForum.Data;
using SimpleForum.Interfaces;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Pages.Threads;

public record ReplyRequest(string ThreadId, string Content);

public class ReplyHandler : IRequestHandler<ReplyRequest, Result>
{
    private readonly SimpleForumContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public ReplyHandler(SimpleForumContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<Result> Handle(ReplyRequest param, CancellationToken cancellationToken = default)
    {
        if (_userAccessor.User == null) return Result.Failure("User not signed in");
        if (String.IsNullOrWhiteSpace(param.Content)) return Result.Failure("Reply cannot be blank");
        
        await _context.Replies.AddAsync(new ForumReply(param.Content, param.ThreadId, _userAccessor.User.Username),
            cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful();
    }
}