using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Interfaces;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages.Threads;

public class View : PageModel
{
    public PageData? Data { get; set; }
    
    public async Task<IActionResult> OnGet(string threadId, [FromServices] IRequestHandler<RequestModel, Result<ForumThread>> handler)
    {
        // Retrieves thread, returning 404 if not found
        Result<ForumThread> result = await handler.Handle(new RequestModel(threadId));
        if (result.Success && result.Value != null)
        {
            Data = new PageData(result.Value);
            return Page();
        }

        return NotFound();
    }

    public record PageData(ForumThread Thread);
    
    public record RequestModel(string ThreadId);
    
    public class Handler : IRequestHandler<RequestModel, Result<ForumThread>>
    {
        private readonly SimpleForumContext _context;

        public Handler(SimpleForumContext context)
        {
            _context = context;
        }

        // Queries the database for a forum thread with the specified Id
        public async Task<Result<ForumThread>> Handle(RequestModel param, CancellationToken cancellationToken = default)
        {
            ForumThread? thread = await _context.Threads
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.ThreadId == param.ThreadId, cancellationToken);

            return thread != null ? Result.Successful(thread) : Result.Failure<ForumThread>("Thread not found");
        }
    }
}