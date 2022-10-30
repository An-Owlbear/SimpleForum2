using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Queries.Users;
using SimpleForum.Util;

namespace SimpleForum.Pages.Users.Comments;

[Authorize]
public class Delete : PageModel
{
    private readonly IMediator _mediator;

    public Delete(IMediator mediator) => _mediator = mediator;
    
    public async Task<IActionResult> OnPost(string id, string redirectUrl)
    {
        Result result = await _mediator.Send(new DeleteProfileCommentRequest(id));
        if (result.Success) return Redirect(redirectUrl);
        return BadRequest();
    }
}