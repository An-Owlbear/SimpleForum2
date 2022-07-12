using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;
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

    public record ViewUserRequest(string UserId) : IRequest<Result<ViewUserResponse>>;

    public record ViewUserResponse
    {
        public string Username { get; } = default!;
        public string ProfileImage { get; } = default!;
        public DateTime DateJoined = default!;
        public IEnumerable<ForumThread> RecentThreads { get; } = default!;
        public IEnumerable<ForumReply> RecentReplies { get; } = default!;

        public ViewUserResponse(Models.User user)
        {
            Username = user.Username;
            ProfileImage = user.ProfileImage;
            DateJoined = user.DateJoined;
            RecentThreads = user.Threads;
            RecentReplies = user.Replies;
        }
    }

    public class ViewUserHandler : IRequestHandler<ViewUserRequest, Result<ViewUserResponse>>
    {
        private readonly SimpleForumContext _context;


        public ViewUserHandler(SimpleForumContext context)
        {
            _context = context;
        }

        public async Task<Result<ViewUserResponse>> Handle(ViewUserRequest request, CancellationToken cancellationToken)
        {
            User? user = await _context.Users
                .Include(u => u.Threads
                    .OrderByDescending(t => t.DatePosted)
                    .Take(5))
                .ThenInclude(t => t.Forum)
                .Include(u => u.Replies
                    .OrderByDescending(r => r.DatePosted)
                    .Take(5))
                .ThenInclude(r => r.Thread)
                .ThenInclude(t => t.Forum)
                .FirstOrDefaultAsync(u => u.Username == request.UserId, cancellationToken);

            return user == null
                ? Result.Failure<ViewUserResponse>("User not found")
                : Result.Successful(new ViewUserResponse(user));
        }
    }
}