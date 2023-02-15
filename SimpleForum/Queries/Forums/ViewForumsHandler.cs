using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;

namespace SimpleForum.Queries.Forums;

/// <summary>
/// Retrieves a list of forums
/// </summary>
public record ViewForumsRequest : IRequest<ViewForumsResponse>;

/// <summary>
/// List of forums
/// </summary>
/// <param name="Forums">List of forums</param>
public record ViewForumsResponse(IEnumerable<Forum> Forums);

public class ViewForumsHandler : IRequestHandler<ViewForumsRequest, ViewForumsResponse>
{
    private readonly SimpleForumContext _context;

    public ViewForumsHandler(SimpleForumContext context)
    {
        _context = context;
    }

    public async Task<ViewForumsResponse> Handle(ViewForumsRequest request, CancellationToken cancellationToken)
    {
        // Retrieves and returns a list of available forums
        IEnumerable<Forum> forums = await _context.Forums.ToListAsync(cancellationToken);
        return new ViewForumsResponse(forums);
    }
}