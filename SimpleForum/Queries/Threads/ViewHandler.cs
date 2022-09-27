using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Threads;

public record ViewRequest(string ThreadId) : IRequest<Result<ForumThread>>;

public class ViewHandler : IRequestHandler<ViewRequest, Result<ForumThread>>
{
    private readonly SimpleForumContext _context;

    public ViewHandler(SimpleForumContext context)
    {
        _context = context;
    }

    // Queries the database for a forum thread with the specified Id
    public async Task<Result<ForumThread>> Handle(ViewRequest param, CancellationToken cancellationToken = default)
    {
        ForumThread? thread = await _context.Threads
            .Include(t => t.User)
            .Include(t => t.Replies)
            .FirstOrDefaultAsync(t => t.ThreadId == param.ThreadId, cancellationToken);

        if (thread == null) return Result.Failure<ForumThread>("Thread not found");

        return Result.Successful(thread);
    }
}