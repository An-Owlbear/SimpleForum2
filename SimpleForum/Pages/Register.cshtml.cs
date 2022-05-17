using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Data;
using SimpleForum.Interfaces;
using SimpleForum.Models;

namespace SimpleForum.Pages;

public class Register : PageModel
{
    private readonly IRequestHandler<RequestModel, Result> _handler;

    public Register(IRequestHandler<RequestModel, Result> handler)
    {
        _handler = handler;
    }

    public void OnGet() { }
    
    public async Task<IActionResult> OnPost(RequestModel model)
    {
        Result result = await _handler.Handle(model);
        if (result == Result.Success) return RedirectToPage("Index");
        return RedirectToPage("Register", new { error = result });
    }

    public record RequestModel(string Username, string Email, string Password, string ConfirmPassword);
    
    public enum Result
    {
        MissingDetails,
        EmailInUse,
        DifferentPasswords,
        Success
    }

    public class Handler : IRequestHandler<RequestModel, Result>
    {
        private readonly SimpleForumContext _context;

        public Handler(SimpleForumContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(RequestModel param, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(param.Email) || String.IsNullOrEmpty(param.Username) || String.IsNullOrEmpty(param.Password))
                return Result.MissingDetails;

            if (param.Password != param.ConfirmPassword) return Result.DifferentPasswords;

            if (_context.Users.Any(u => u.Email == param.Email)) return Result.EmailInUse;

            await _context.Users.AddAsync(new User(param.Username, param.Email, param.Password), cancellationToken);
            await _context.SaveChangesAsync();
            return Result.Success;
        }
    }
}