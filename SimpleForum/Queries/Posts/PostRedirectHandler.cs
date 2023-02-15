using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Posts;

/// <summary>
/// Requests information for the thread to redirect to containing the given post Id
/// </summary>
/// <param name="Id">The Id of the post to search for the containing thread of</param>
public record PostRedirectRequest(string Id) : IRequest<Result<PostRedirectResponse>>;

/// <summary>
/// Contains information of the thread to redirect to
/// </summary>
/// <param name="ThreadId">The ID of thread</param>
/// <param name="PageNo">The page number for given post is on</param>
/// <param name="ReplyId">The Id of the reply, the same Id as <see cref="PostRedirectRequest.Id"/></param>
public record PostRedirectResponse(string ThreadId, int PageNo, string ReplyId);
    
/// <summary>
/// Determines the information of the thread to redirect to for the given post id
/// </summary>
public class PostRedirectHandler : IRequestHandler<PostRedirectRequest, Result<PostRedirectResponse>>
{
    private readonly SimpleForumContext _context;

    public PostRedirectHandler(SimpleForumContext context)
    {
        _context = context;
    }

    public async Task<Result<PostRedirectResponse>> Handle(PostRedirectRequest request, CancellationToken cancellationToken)
    {
        // Retrieves the post, returning error if none found 
        ForumReply? reply = await _context.Replies.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);
        return reply == null 
            ? Result.Failure<PostRedirectResponse>("Post not found", ErrorType.NotFound) 
            : Result.Successful(new PostRedirectResponse(reply.ThreadId, 0, reply.ReplyId));
    }
}