using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Queries.Threads;

// Request object for deleting a thread
public record DeleteThreadRequest(string Id) : IRequest<Result>;

// Handler class for deleting threads
public class DeleteThreadHandler : IRequestHandler<DeleteThreadRequest, Result>
{
    private readonly SimpleForumContext _context;
    private readonly ICurrentUserAccessor _userAccessor;

    public DeleteThreadHandler(SimpleForumContext context, ICurrentUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<Result> Handle(DeleteThreadRequest request, CancellationToken cancellationToken)
    {
        // Retrieves thread and verifies the user has permission to delete it
        ForumThread? thread = await _context.Threads.FindAsync(new object[] { request.Id }, cancellationToken);
        if (thread == null) return Result.Failure("Thread not found");
        if (thread.UserId != _userAccessor.User?.Username) return Result.Failure("Permission denied");

        // Clears thread information (title, content) and assigns to ghost user
        thread.Title = "";
        thread.Content = "";
        thread.UserId = Constants.DeletedUser.Id;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Successful();
    }
}