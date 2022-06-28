using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;

namespace SimpleForum.Pages;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public ForumsResponse Data { get; set; }

    public IndexModel(IMediator mediator) => _mediator = mediator;

    public async Task OnGet()
    {
        Data = await _mediator.Send(new ForumsRequest());
    }
    
    public record ForumsRequest : IRequest<ForumsResponse>;

    public record ForumsResponse(IEnumerable<Forum> Forums);

    public class Handler : IRequestHandler<ForumsRequest, ForumsResponse>
    {
        private readonly SimpleForumContext _context;

        public Handler(SimpleForumContext context)
        {
            _context = context;
        }

        public async Task<ForumsResponse> Handle(ForumsRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<Forum> forums = await _context.Forums.ToListAsync(cancellationToken);
            return new ForumsResponse(forums);
        }
    }
}
