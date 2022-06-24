using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Interfaces;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages.Threads;

public record ViewRequest(string ThreadId);
    
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
            .FirstOrDefaultAsync(t => t.ThreadId == param.ThreadId, cancellationToken);

        return thread != null ? Result.Successful(thread) : Result.Failure<ForumThread>("Thread not found");
    }
}