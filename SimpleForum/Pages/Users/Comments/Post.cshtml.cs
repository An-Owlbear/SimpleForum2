using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Queries.Users;
using SimpleForum.Util;

namespace SimpleForum.Pages.Users;

public class Post : PageModel
{
    private readonly IMediator _mediator;

    public Post(IMediator mediator) => _mediator = mediator;

    public record CommentPostModel(string content, string redirectUrl);
    
    public async Task<IActionResult> OnPost(string id, CommentPostModel request)
    {
        Result<PostProfileCommentResponse> result =
            await _mediator.Send(new PostProfileCommentRequest(id, request.content));

        if (result.Success) return RedirectToPage("/Users/Index", new { userId = id });
        return Redirect(request.redirectUrl);
    }
}