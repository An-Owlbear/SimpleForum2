using System.ComponentModel.DataAnnotations;

namespace SimpleForum.Models;

public class ForumReply
{
    private ForumReply(string replyId, string content, DateTime datePosted, string threadId, string userId)
    {
        ReplyId = replyId;
        Content = content;
        DatePosted = datePosted;
        ThreadId = threadId;
        UserId = userId;
    }

    public ForumReply(string content, string threadId, string userId, DateTime? dateTime = null)
    {
        ReplyId = Guid.NewGuid().ToString();
        Content = content;
        DatePosted = dateTime ?? DateTime.UtcNow;
        ThreadId = threadId;
        UserId = userId;
    }
    
    [Key]
    public string ReplyId { get; set; }
    public string Content { get; set; }
    public DateTime DatePosted { get; set; }
    public string ThreadId { get; set; }
    public string UserId { get; set; }

    public ForumThread Thread { get; set; } = null!;
    public User User { get; set; } = null!;
}