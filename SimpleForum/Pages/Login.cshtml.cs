using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Data;
using SimpleForum.Extensions;
using SimpleForum.Interfaces;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages;

public class Login : PageModel
{
    public PageData? Data { get; set; }
    
    public void OnGet() { }

    public async Task<IActionResult> OnPost(RequestModel request, [FromServices] IRequestHandler<RequestModel, Result> handler)
    {
        Result result = await handler.Handle(request);
        if (result.Success) return RedirectToPage("Index");

        Data = new PageData(result.Error);
        return Page();
    }

    public record PageData(string? Error);

    public record RequestModel(string Username, string Password);

    public class Handler : IRequestHandler<RequestModel, Result>
    {
        private readonly SimpleForumContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(SimpleForumContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(RequestModel param, CancellationToken cancellationToken = default)
        {
            User? user = _context.Users.FirstOrDefault(u =>
                u.Username == param.Username || u.Email == param.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(param.Password, user.PasswordHash)) 
                return Result.Failure("Incorrect username or password");

            await _httpContextAccessor.HttpContext.SignInAsync(user);
            return Result.Successful();
        }
    }
}