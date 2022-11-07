using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Queries.Users;
using SimpleForum.Util;

namespace SimpleForum.Pages.Users.Comments;

public class Index : PageModel
{
    private readonly IMediator _mediator; 
    
    public ProfileCommentsResponse Data { get; set; } = null!;

    public Index(IMediator mediator) => _mediator = mediator;
    
    public async Task<IActionResult> OnGet(string id)
    {
        Result<ProfileCommentsResponse> result = await _mediator.Send(new ProfileCommentsRequest(id));
        if (result.Success && result.Value != null)
        {
            Data = result.Value;
            return Page();
        }

        return NotFound();
    }
}