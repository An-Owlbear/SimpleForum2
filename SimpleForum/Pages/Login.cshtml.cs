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
public class Login : PageModel
{
    private readonly IMediator _mediator;

    public Login(IMediator mediator) => _mediator = mediator;
    
    public PageData? Data { get; set; }
    
    public void OnGet() { }

    public async Task<IActionResult> OnPost(RequestModel request)
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

    public record RequestModel(string Username, string Password) : IRequest<Result<User>>;

    public class Handler : IRequestHandler<RequestModel, Result<User>>
    {
        private readonly SimpleForumContext _context;

        public Handler(SimpleForumContext context)
        {
            _context = context;
        }

        public Task<Result<User>> Handle(RequestModel param, CancellationToken cancellationToken = default)
        {
            User? user = _context.Users.FirstOrDefault(u =>
                u.Username == param.Username || u.Email == param.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(param.Password, user.PasswordHash)) 
                return Task.FromResult(Result.Failure<User>("Incorrect username or password", ErrorType.BadRequest));
            
            return Task.FromResult(Result.Successful(user));
        }
    }
}