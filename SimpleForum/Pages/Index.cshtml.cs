using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Queries.Forums;

namespace SimpleForum.Pages;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public ViewForumsResponse Data { get; set; } = null!;

    public IndexModel(IMediator mediator) => _mediator = mediator;

    public async Task OnGet()
    {
        Data = await _mediator.Send(new ViewForumsRequest());
    }
}
