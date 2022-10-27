using System.Text.RegularExpressions;
using MediatR;
using SimpleForum.Data;
using SimpleForum.Models;
using SimpleForum.Util;

namespace SimpleForum.Queries.Users;

public record RegisterRequestModel
    (string Username, string Email, string Password, string ConfirmPassword) : IRequest<Result<User>>;

public class RegisterHandler : IRequestHandler<RegisterRequestModel, Result<User>>
{
    private readonly SimpleForumContext _context;

    public RegisterHandler(SimpleForumContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> Handle(RegisterRequestModel param, CancellationToken cancellationToken = default)
    {
        // Checks given data for errors and signs up user if there is none
        if (String.IsNullOrEmpty(param.Email) || String.IsNullOrEmpty(param.Username) || String.IsNullOrEmpty(param.Password))
            return Result.Failure<User>("Please enter all details", ErrorType.BadRequest);

        if (!Regex.IsMatch(param.Email, @"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$"))
            return Result.Failure<User>("Please enter a valid email address", ErrorType.BadRequest);

        if (param.Password != param.ConfirmPassword) return Result.Failure<User>("The entered passwords do not match", ErrorType.BadRequest);

        if (_context.Users.Any(u => u.Email == param.Email)) return Result.Failure<User>("The entered email is in use", ErrorType.BadRequest);

        User userToAdd = new (param.Username, param.Email, param.Password);
        await _context.Users.AddAsync(userToAdd, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Successful(userToAdd);
    }
}