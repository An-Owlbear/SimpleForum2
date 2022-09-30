using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;

namespace SimpleForum.Queries.Forums;

public record ViewForumsRequest : IRequest<ViewForumsResponse>;

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
        IEnumerable<Forum> forums = await _context.Forums.ToListAsync(cancellationToken);
        return new ViewForumsResponse(forums);
    }
}