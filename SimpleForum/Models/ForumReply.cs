using System.ComponentModel.DataAnnotations;

namespace SimpleForum.Models;

public class ForumReply
{
    private ForumReply(string replyId, string content, DateTime datePosted, bool isOpeningPost, string threadId,
        string userId)
    {
        ReplyId = replyId;
        Content = content;
        DatePosted = datePosted;
        IsOpeningPost = isOpeningPost;
        ThreadId = threadId;
        UserId = userId;
    }

    public ForumReply(string content, bool isOpeningPost, string threadId, string userId, DateTime? dateTime = null)
    {
        ReplyId = Guid.NewGuid().ToString();
        Content = content;
        DatePosted = dateTime ?? DateTime.UtcNow;
        IsOpeningPost = isOpeningPost;
        ThreadId = threadId;
        UserId = userId;
    }
    
    [Key]
    public string ReplyId { get; set; }
    public string Content { get; set; }
    public DateTime DatePosted { get; set; }
    public bool IsOpeningPost { get; set; }
    public string ThreadId { get; set; }
    public string UserId { get; set; }

    public ForumThread Thread { get; set; } = null!;
    public User User { get; set; } = null!;
}