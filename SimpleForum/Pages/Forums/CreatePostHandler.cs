using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Pages.Forums;

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
        if (_userAccessor.User == null) return Result.Failure<ForumThread>("User not signed in");
        if (String.IsNullOrWhiteSpace(param.Title) || String.IsNullOrWhiteSpace(param.Content))
            return Result.Failure<ForumThread>("Title and content must cannot be empty");

        Forum? forum = await _context.Forums.FindAsync(new object[] { param.ForumId }, cancellationToken);
        if (forum == null) return Result.Failure<ForumThread>("Forum not found");

        ForumThread thread = new(param.Title, _userAccessor.User.Username, forum.ForumId);
        ForumReply reply = new(param.Content, thread.ThreadId, _userAccessor.User.Username, thread.DatePosted);
        await _context.Threads.AddAsync(thread, cancellationToken);
        await _context.Replies.AddAsync(reply, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful(thread);
    }
}