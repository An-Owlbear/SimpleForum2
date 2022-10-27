using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Users;

public record LoginRequestModel(string Username, string Password) : IRequest<Result<User>>;

public class LoginHandler : IRequestHandler<LoginRequestModel, Result<User>>
{
    private readonly SimpleForumContext _context;

    public LoginHandler(SimpleForumContext context)
    {
        _context = context;
    }

    public Task<Result<User>> Handle(LoginRequestModel param, CancellationToken cancellationToken = default)
    {
        User? user = _context.Users.FirstOrDefault(u =>
            u.Username == param.Username || u.Email == param.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(param.Password, user.PasswordHash)) 
            return Task.FromResult(Result.Failure<User>("Incorrect username or password", ErrorType.BadRequest));
            
        return Task.FromResult(Result.Successful(user));
    }
}