using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages.Forums;

public class Index : PageModel
{
    private readonly IMediator _mediator;

    public ViewForumResponse Data { get; set; } = null!;

    public Index(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> OnGet(string forumId)
    {
        Result<ViewForumResponse> result = await _mediator.Send(new ViewForumRequest(forumId));
        if (result.Success && result.Value != null)
        {
            Data = result.Value;
            return Page();
        }

        return NotFound();
    }

    public record ViewForumRequest(string ForumID) : IRequest<Result<ViewForumResponse>>;

    public record ViewForumResponse(string ForumId, string Name, IEnumerable<ForumThread> Threads);

    public class ViewForumHandler : IRequestHandler<ViewForumRequest, Result<ViewForumResponse>>
    {
        private readonly SimpleForumContext _context;

        public ViewForumHandler(SimpleForumContext context)
        {
            _context = context;
        }

        public async Task<Result<ViewForumResponse>> Handle(ViewForumRequest request, CancellationToken cancellationToken)
        {
            // Retrieves forum, returning an error if null, and the name and replies if not
            Forum? forum = await _context.Forums
                .Include(f => f.Threads)
                .FirstOrDefaultAsync(f => f.ForumId == request.ForumID, cancellationToken);

            return forum == null
                ? Result.Failure<ViewForumResponse>("Forum not found")
                : Result.Successful(new ViewForumResponse(forum.ForumId, forum.Name, forum.Threads));
        }
    }
}