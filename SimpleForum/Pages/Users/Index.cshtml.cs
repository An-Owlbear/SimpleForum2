using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Queries.Users;
using SimpleForum.Util;

namespace SimpleForum.Pages.Users;

public class Index : PageModel
{
    private readonly IMediator _mediator;

    public ViewUserResponse Data { get; set; } = null!;

    public Index(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> OnGet(string userId)
    {
        Result<ViewUserResponse> result = await _mediator.Send(new ViewUserRequest(userId));
        if (result.Success && result.Value != null)
        {
            Data = result.Value;
            return Page();
        }

        return NotFound();
    }
}