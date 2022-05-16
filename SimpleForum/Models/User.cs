namespace SimpleForum.Models;

public class User
{
    public User(string id, string email, string passwordHash, DateTime dateJoined)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        DateJoined = dateJoined;
    }

    public string Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; } 
    public DateTime DateJoined { get; set; }
}