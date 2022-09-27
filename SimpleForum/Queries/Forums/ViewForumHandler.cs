using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Forums;

public record ViewForumRequest(string ForumID) : IRequest<Result<ViewForumResponse>>;

public record ViewForumResponse(string ForumId, string Name, IEnumerable<ForumThread> Threads);

public class ViewForumHandler : IRequestHandler<ViewForumRequest, Result<ViewForumResponse>>
{
    private readonly SimpleForumContext _context;

    public ViewForumHandler(SimpleForumContext context)
    {
        _context = context;
    }

    public async Task<Result<ViewForumResponse>> Handle(ViewForumRequest request,
        CancellationToken cancellationToken)
    {
        // Retrieves forum, returning an error if null, and the name and replies if not
        Forum? forum = await _context.Forums
            .Include(f => f.Threads.Where(t => t.UserId != Constants.DeletedUser.Id))
            .FirstOrDefaultAsync(f => f.ForumId == request.ForumID, cancellationToken);

        return forum == null
            ? Result.Failure<ViewForumResponse>("Forum not found")
            : Result.Successful(new ViewForumResponse(forum.ForumId, forum.Name, forum.Threads));
    }
}