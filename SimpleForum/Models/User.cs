using System.ComponentModel.DataAnnotations;

namespace SimpleForum.Models;

public class User
{
    // Constructor used by EF Core when retrieving users from the database
    public User(string username, string email, string passwordHash, DateTime dateJoined, string profileImage)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        DateJoined = dateJoined;
        ProfileImage = profileImage;
    }

    // Constructor used when creating a new user
    public User(string username, string email, string password)
    {
        Username = username;
        Email = email;
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        DateJoined = DateTime.UtcNow;
        ProfileImage = "default_pfp";
    }

    [Key]
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; } 
    public DateTime DateJoined { get; set; }
    public string ProfileImage { get; set; }
}