using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Data;
using SimpleForum.Interfaces;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages;

public class Register : PageModel
{
    private readonly IRequestHandler<RequestModel, Result> _handler;

    public PageData Data { get; private set; } = new(null);

    public Register(IRequestHandler<RequestModel, Result> handler)
    {
        _handler = handler;
    }

    public void OnGet() { }
    
    public async Task<IActionResult> OnPost(RequestModel model)
    {
        Result result = await _handler.Handle(model);
        if (result.Success) return RedirectToPage("Index");
        
        Data = new PageData(result.Error);
        return Page();
    }

    public record PageData(string? Error);
    
    public record RequestModel(string Username, string Email, string Password, string ConfirmPassword);

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
            // Checks given data for errors and signs up user if there is none
            if (String.IsNullOrEmpty(param.Email) || String.IsNullOrEmpty(param.Username) || String.IsNullOrEmpty(param.Password))
                return Result.Failure("Please enter all details");

            if (!Regex.IsMatch(param.Email, "/^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$/"))
                return Result.Failure("Please enter a valid email address");

            if (param.Password != param.ConfirmPassword) return Result.Failure("The entered passwords do not match");

            if (_context.Users.Any(u => u.Email == param.Email)) return Result.Failure("The entered email is in use");

            await _context.Users.AddAsync(new User(param.Username, param.Email, param.Password), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            // Sets authentication cookie
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, param.Username)
            };
            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            
            return Result.Successful();
        }
    }
}