using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Models;
using SimpleForum.Queries.Threads;
using SimpleForum.Util;

namespace SimpleForum.Pages.Threads;

public class View : PageModel
{
    private IMediator _mediator;

    public View(IMediator mediator) => _mediator = mediator;

    public ForumThread Data { get; set; } = null!;
    public string? ReplyError { get; set; }
    
    public async Task<IActionResult> OnGet(string threadId)
    {
        // Retrieves thread, returning 404 if not found
        Result<ForumThread> result = await _mediator.Send(new ViewRequest(threadId));
        if (result.Success && result.Value != null)
        {
            Data = result.Value;
            return Page();
        }

        return NotFound();
    }

    public record ReplyPostModel(string Content);

    public async Task<IActionResult> OnPostReply(string threadId, ReplyPostModel reply)
    {
        if (User.Identity is not { IsAuthenticated: true }) return Unauthorized();
        Result<ForumReply> result = await _mediator.Send(new ReplyRequest(threadId, reply.Content));
        
        // Redirects to reply if successful, else returns error
        if (result.Success)
        {
            string url = Url.Page("/Threads/View", new { threadId }) + $"#r{result.Value?.ReplyId}";
            return Redirect(url);
        }
        
        ReplyError = result.Error;
        return await OnGet(threadId);
    }
}