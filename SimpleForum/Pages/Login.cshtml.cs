﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleForum.Data;
using SimpleForum.Extensions;
using SimpleForum.Filters;
using SimpleForum.Interfaces;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Pages;

[Unauthorized]
public class Login : PageModel
{
    public PageData? Data { get; set; }
    
    public void OnGet() { }

    public async Task<IActionResult> OnPost(RequestModel request, [FromServices] IRequestHandler<RequestModel, Result<User>> handler)
    {
        Result<User> result = await handler.Handle(request);
        if (result.Success && result.Value != null)
        {
            await HttpContext.SignInAsync(result.Value);
            return RedirectToPage("Index");
        }

        Data = new PageData(result.Error);
        return Page();
    }

    public record PageData(string? Error);

    public record RequestModel(string Username, string Password);

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
                return Task.FromResult(Result.Failure<User>("Incorrect username or password"));
            
            return Task.FromResult(Result.Successful(user));
        }
    }
}