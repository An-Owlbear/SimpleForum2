using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages.Posts;

public class Index : PageModel
{
    private readonly IMediator _mediator;

    public Index(IMediator mediator) => _mediator = mediator;

    public async Task<IActionResult> OnGet(string id)
    {
        Result<PostRedirectResponse> result = await _mediator.Send(new PostRedirectRequest(id));
        if (result.Success && result.Value != null)
            return Redirect(Url.Page("/Threads/View", new { threadId = result.Value.ThreadId }) +
                            $"#r{result.Value.ReplyId}");

        return NotFound();

    }

    public record PostRedirectRequest(string Id) : IRequest<Result<PostRedirectResponse>>;

    public record PostRedirectResponse(string ThreadId, int PageNo, string ReplyId);
    
    public class PostRedirectHandler : IRequestHandler<PostRedirectRequest, Result<PostRedirectResponse>>
    {
        private readonly SimpleForumContext _context;

        public PostRedirectHandler(SimpleForumContext context)
        {
            _context = context;
        }

        public async Task<Result<PostRedirectResponse>> Handle(PostRedirectRequest request, CancellationToken cancellationToken)
        {
            ForumReply? reply = await _context.Replies.FindAsync(request.Id);
            if (reply == null) return Result.Failure<PostRedirectResponse>("Post not found");
            return Result.Successful(new PostRedirectResponse(reply.ThreadId, 0, reply.ReplyId));
        }
    }
}