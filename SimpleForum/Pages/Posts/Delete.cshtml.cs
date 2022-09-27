using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Queries.Posts;
using SimpleForum.Util;

namespace SimpleForum.Pages.Posts;

public class Delete : PageModel
{
    private readonly IMediator _mediator;

    public Delete(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> OnPost(string id, string redirectUrl)
    {
        Result result = await _mediator.Send(new DeletePostRequest(id));
        if (result.Success) return Redirect(redirectUrl);
        return BadRequest();
    }
}