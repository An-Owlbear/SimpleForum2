using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Models;
using SimpleForum.Queries.Forums;
using SimpleForum.Util;

namespace SimpleForum.Pages.Forums;

[Authorize]
public class Create : PageModel
{
    private readonly IMediator _mediator;

    public CreateFormResponse Data { get; set; } = null!;

    public Create(IMediator mediator) => _mediator = mediator;
    
    public string? CreateError { get; set; }

    public async Task<IActionResult> OnGet(string forumId)
    {
        Result<CreateFormResponse> result = await _mediator.Send(new CreateFormRequest(forumId));
        if (result.Success && result.Value != null)
        {
            Data = result.Value;
            return Page();
        }

        return NotFound();
    }

    public async Task<IActionResult> OnPost(CreatePostRequest model)
    {
        Result<ForumThread> result = await _mediator.Send(model);
        if (result.Success && result.Value != null)
            return RedirectToPage("/Threads/View", new { threadId = result.Value.ThreadId });

        CreateError = result.Error.Detail;
        return Page();
    }
}