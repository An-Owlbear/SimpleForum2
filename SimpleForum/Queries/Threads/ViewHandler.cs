using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Threads;

/// <summary>
/// Retrieves a thread and it's replies
/// </summary>
/// <param name="ThreadId">The id of the thread to retrieve</param>
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
        // Retrieves the thread including the replies and user information
        ForumThread? thread = await _context.Threads
            .Include(t => t.User)
            .Include(t => t.Replies)
            .FirstOrDefaultAsync(t => t.ThreadId == param.ThreadId, cancellationToken);

        // Returns an error if the thread is not found
        return thread == null 
            ? Result.Failure<ForumThread>("Thread not found", ErrorType.NotFound) 
            : Result.Successful(thread);
    }
}