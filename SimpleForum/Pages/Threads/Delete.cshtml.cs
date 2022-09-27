using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Queries.Threads;
using SimpleForum.Util;

namespace SimpleForum.Pages.Threads;

public class Delete : PageModel
{
    private readonly IMediator _mediator;

    public Delete(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> OnPost(string id)
    {
        // Sends request displaying error if it fails
        Result result = await _mediator.Send(new DeleteThreadRequest(id));
        if (result.Success) return Redirect("/");
        return BadRequest();
    }
}