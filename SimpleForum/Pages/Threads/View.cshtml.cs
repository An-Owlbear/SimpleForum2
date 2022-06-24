using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Interfaces;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages.Threads;

public class View : PageModel
{
    public ForumThread? Thread { get; set; }
    public string? ReplyError { get; set; }
    
    public async Task<IActionResult> OnGet(string threadId, [FromServices] IRequestHandler<ViewRequest, Result<ForumThread>> handler)
    {
        // Retrieves thread, returning 404 if not found
        Result<ForumThread> result = await handler.Handle(new ViewRequest(threadId));
        if (result.Success && result.Value != null)
        {
            Thread = result.Value;
            return Page();
        }

        return NotFound();
    }

    public record ReplyPostModel(string Content);

    public async Task<IActionResult> OnPostReply(string threadId, ReplyPostModel reply,
        [FromServices] IRequestHandler<ReplyRequest, Result> handler)
    {
        if (User.Identity is not { IsAuthenticated: true }) return Unauthorized();
        Result result = await handler.Handle(new ReplyRequest(threadId, reply.Content));
        
        // Redirects to reply if successful, else returns error
        if (result.Success) return RedirectToPage($"/Threads/{threadId}");
        
        ReplyError = result.Error;
        return Page();
    }
}