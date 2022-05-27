using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Data;
using SimpleForum.Interfaces;
using SimpleForum.Models;
using SimpleForum.Services;
using SimpleForum.Util;

namespace SimpleForum.Pages.Thread;

[Authorize]
public class Create : PageModel
{
    public PageData? Data { get; set; }
    
    public void OnGet() { }

    public async Task<IActionResult> OnPost(RequestModel model, [FromServices] IRequestHandler<RequestModel, Result<ForumThread>> handler)
    {
        Result<ForumThread> result = await handler.Handle(model);
        if (result.Success && result.Value != null)
            return RedirectToPage("/Thread", new { id = result.Value.ThreadId });

        Data = new PageData(result.Error);
        return Page();
    }

    public record PageData(string? Error);

    public record RequestModel(string Title, string Content);
    
    public class Handler : IRequestHandler<RequestModel, Result<ForumThread>>
    {
        private readonly SimpleForumContext _context;
        private readonly ICurrentUserAccessor _userAccessor;

        public Handler(SimpleForumContext context, ICurrentUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Result<ForumThread>> Handle(RequestModel param, CancellationToken cancellationToken = default)
        {
            if (_userAccessor.User == null) return Result.Failure<ForumThread>("User not signed in");
            if (String.IsNullOrWhiteSpace(param.Title) || String.IsNullOrWhiteSpace(param.Content))
                return Result.Failure<ForumThread>("Title and content must cannot be empty");

            ForumThread thread = new(param.Title, param.Content, _userAccessor.User.Username);
            await _context.Threads.AddAsync(thread, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Successful(thread);
        }
    }
}