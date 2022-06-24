using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Data;
using SimpleForum.Extensions;
using SimpleForum.Filters;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages;

[Unauthorized]
public class Register : PageModel
{
    private readonly IMediator _mediator;

    public Register(IMediator mediator) => _mediator = mediator;
    
    public PageData Data { get; private set; } = new(null);

    public void OnGet() { }
    
    public async Task<IActionResult> OnPost(RequestModel model)
    {
        Result<User> result = await _mediator.Send(model);
        if (result.Success && result.Value != null)
        {
            await HttpContext.SignInAsync(result.Value);
            return RedirectToPage("/Index");
        }
        
        Data = new PageData(result.Error);
        return Page();
    }

    public record PageData(string? Error);

    public record RequestModel
        (string Username, string Email, string Password, string ConfirmPassword) : IRequest<Result<User>>;

    public class Handler : IRequestHandler<RequestModel, Result<User>>
    {
        private readonly SimpleForumContext _context;

        public Handler(SimpleForumContext context)
        {
            _context = context;
        }

        public async Task<Result<User>> Handle(RequestModel param, CancellationToken cancellationToken = default)
        {
            // Checks given data for errors and signs up user if there is none
            if (String.IsNullOrEmpty(param.Email) || String.IsNullOrEmpty(param.Username) || String.IsNullOrEmpty(param.Password))
                return Result.Failure<User>("Please enter all details");

            if (!Regex.IsMatch(param.Email, @"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$"))
                return Result.Failure<User>("Please enter a valid email address");

            if (param.Password != param.ConfirmPassword) return Result.Failure<User>("The entered passwords do not match");

            if (_context.Users.Any(u => u.Email == param.Email)) return Result.Failure<User>("The entered email is in use");

            User userToAdd = new (param.Username, param.Email, param.Password);
            await _context.Users.AddAsync(userToAdd, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Successful(userToAdd);
        }
    }
}