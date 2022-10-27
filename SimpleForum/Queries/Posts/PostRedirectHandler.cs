using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Posts;

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
        if (reply == null) return Result.Failure<PostRedirectResponse>("Post not found", ErrorType.NotFound);
        return Result.Successful(new PostRedirectResponse(reply.ThreadId, 0, reply.ReplyId));
    }
}