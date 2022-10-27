using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Extensions;
using SimpleForum.Filters;
using SimpleForum.Models;
using SimpleForum.Queries.Users;
using SimpleForum.Util;

namespace SimpleForum.Pages;

[Unauthorized]
public class Login : PageModel
{
    private readonly IMediator _mediator;

    public Login(IMediator mediator) => _mediator = mediator;
    
    public PageData? Data { get; set; }
    
    public void OnGet() { }

    public async Task<IActionResult> OnPost(LoginRequestModel request)
    {
        Result<User> result = await _mediator.Send(request);
        if (result.Success && result.Value != null)
        {
            await HttpContext.SignInAsync(result.Value);
            return RedirectToPage("/Index");
        }

        Data = new PageData(result.Error.Detail);
        return Page();
    }

    public record PageData(string? Error);
}