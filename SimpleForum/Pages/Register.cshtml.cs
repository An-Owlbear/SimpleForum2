using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Data;
using SimpleForum.Extensions;
using SimpleForum.Filters;
using SimpleForum.Models;
using SimpleForum.Queries.Users;
using SimpleForum.Util;

namespace SimpleForum.Pages;

[Unauthorized]
public class Register : PageModel
{
    private readonly IMediator _mediator;

    public Register(IMediator mediator) => _mediator = mediator;
    
    public PageData Data { get; private set; } = new(null);

    public void OnGet() { }
    
    public async Task<IActionResult> OnPost(RegisterRequestModel model)
    {
        Result<User> result = await _mediator.Send(model);
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