using System.ComponentModel.DataAnnotations;

namespace SimpleForum.Models;

public class User
{
    // Constructor used by EF Core when retrieving users from the database
    private User(string username, string email, string passwordHash, DateTime dateJoined, string profileImage, string bio)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        DateJoined = dateJoined;
        ProfileImage = profileImage;
        Bio = bio;
    }

    // Constructor used when creating a new user
    public User(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Bio = string.Empty;
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
    public string Bio { get; set; }

    public List<ForumThread> Threads { get; set; } = null!;
    public List<ForumReply> Replies { get; set; } = null!;
    public List<ProfileComment> PostedProfileComments { get; set; } = null!;
    public List<ProfileComment> ReceivedProfileComments { get; set; } = null!;
}