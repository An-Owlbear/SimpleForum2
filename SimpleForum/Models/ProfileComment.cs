namespace SimpleForum.Models;

public class ProfileComment
{
    public ProfileComment(string profileCommentId, string content, DateTime datePosted, string userId, string recipientProfileId)
    {
        ProfileCommentId = profileCommentId;
        Content = content;
        DatePosted = datePosted;
        UserId = userId;
        RecipientProfileId = recipientProfileId;
    }

    public string ProfileCommentId { get; set; }
    public string Content { get; set; }
    public DateTime DatePosted { get; set; }
    public string UserId { get; set; }
    public string RecipientProfileId { get; set; }

    public User User { get; set; } = null!;
    public User RecipientProfile { get; set; } = null!;
}