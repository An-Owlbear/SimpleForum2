using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Forums;

public record CreateFormRequest(string ForumId) : IRequest<Result<CreateFormResponse>>;

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