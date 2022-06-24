using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages.Threads;

public class View : PageModel
{
    private IMediator _mediator;

    public View(IMediator mediator) => _mediator = mediator;

    public ForumThread? Thread { get; set; }
    public string? ReplyError { get; set; }
    
    public async Task<IActionResult> OnGet(string threadId)
    {
        // Retrieves thread, returning 404 if not found
        Result<ForumThread> result = await _mediator.Send(new ViewRequest(threadId));
        if (result.Success && result.Value != null)
        {
            Thread = result.Value;
            return Page();
        }

        return NotFound();
    }

    public record ReplyPostModel(string Content);

    public async Task<IActionResult> OnPostReply(string threadId, ReplyPostModel reply)
    {
        if (User.Identity is not { IsAuthenticated: true }) return Unauthorized();
        Result result = await _mediator.Send(new ReplyRequest(threadId, reply.Content));
        
        // Redirects to reply if successful, else returns error
        if (result.Success) return RedirectToPage($"/Threads/{threadId}");
        
        ReplyError = result.Error;
        return await OnGet(threadId);
    }
}