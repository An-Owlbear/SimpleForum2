using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages.Threads;

public record ViewRequest(string ThreadId) : IRequest<Result<ViewResponse>>;

public record ViewResponse(string Title, DateTime DatePosted, User User, IEnumerable<ForumReply> Replies);
    
public class ViewHandler : IRequestHandler<ViewRequest, Result<ViewResponse>>
{
    private readonly SimpleForumContext _context;

    public ViewHandler(SimpleForumContext context)
    {
        _context = context;
    }

    // Queries the database for a forum thread with the specified Id
    public async Task<Result<ViewResponse>> Handle(ViewRequest param, CancellationToken cancellationToken = default)
    {
        ForumThread? thread = await _context.Threads
            .Include(t => t.User)
            .Include(t => t.Replies)
            .FirstOrDefaultAsync(t => t.ThreadId == param.ThreadId, cancellationToken);

        if (thread == null) return Result.Failure<ViewResponse>("Thread not found");

        return Result.Successful(new ViewResponse(thread.Title, thread.DatePosted, thread.User, thread.Replies));
    }
}