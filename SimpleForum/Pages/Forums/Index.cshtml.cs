using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Queries.Forums;
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
}