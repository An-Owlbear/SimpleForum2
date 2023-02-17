using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Forums;

/// <summary>
/// Loads the thread creation form for the current forum
/// </summary>
/// <param name="ForumId">The id of the forum to use</param>
public record CreateFormRequest(string ForumId) : IRequest<Result<CreateFormResponse>>;

/// <summary>
/// Information of the forum to display the thread creation form for
/// </summary>
/// <param name="ForumId">The id of the forum</param>
/// <param name="ForumName">The name of the forum</param>
public record CreateFormResponse(string ForumId, string ForumName);

public class CreateFormHandler : IRequestHandler<CreateFormRequest, Result<CreateFormResponse>>
{
    private readonly SimpleForumContext _context;

    public CreateFormHandler(SimpleForumContext context)
    {
        _context = context;
    }

    public async Task<Result<CreateFormResponse>> Handle(CreateFormRequest request, CancellationToken cancellationToken)
    {
        // Retrieves forum, returning an error if none found
        Forum? forum = await _context.Forums.FindAsync(new object[] { request.ForumId }, cancellationToken);
        return forum == null
            ? Result.Failure<CreateFormResponse>("Forum not found", ErrorType.NotFound)
            : Result.Successful(new CreateFormResponse(forum.ForumId, forum.Name));
    }
}