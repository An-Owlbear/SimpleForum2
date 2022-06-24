using System.ComponentModel.DataAnnotations;

namespace SimpleForum.Models;

public class ForumThread
{
    private ForumThread(string threadId, string title, string content, string userId, DateTime datePosted)
    {
        ThreadId = threadId;
        Title = title;
        Content = content;
        UserId = userId;
        DatePosted = datePosted;
    }
    
    public ForumThread(string title, string content, string userId)
    {
        ThreadId = Guid.NewGuid().ToString();
        Title = title;
        Content = content;
        UserId = userId;
        DatePosted = DateTime.UtcNow;
    }

    [Key]
    public string ThreadId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string UserId { get; set; }
    public DateTime DatePosted { get; set; }

    public User User { get; set; } = null!;
}