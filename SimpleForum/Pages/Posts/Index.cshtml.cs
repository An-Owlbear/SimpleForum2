using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Queries.Posts;
using SimpleForum.Util;

namespace SimpleForum.Pages.Posts;

public class Index : PageModel
{
    private readonly IMediator _mediator;

    public Index(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> OnGet(string id)
    {
        Result<PostRedirectResponse> result = await _mediator.Send(new PostRedirectRequest(id));
        if (result.Success && result.Value != null)
            return Redirect(Url.Page("/Threads/View", new { threadId = result.Value.ThreadId }) +
                            $"#r{result.Value.ReplyId}");

        return NotFound();

    }
}