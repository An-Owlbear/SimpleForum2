﻿using System.ComponentModel.DataAnnotations;

namespace SimpleForum.Models;

public class ForumThread
{
    private ForumThread(string threadId, string title, string userId, DateTime datePosted, string forumId)
    {
        ThreadId = threadId;
        Title = title;
        UserId = userId;
        DatePosted = datePosted;
        ForumId = forumId;
    }
    
    public ForumThread(string title, string userId, string forumId, DateTime? dateTime = null)
    {
        ThreadId = Guid.NewGuid().ToString();
        Title = title;
        UserId = userId;
        ForumId = forumId;
        DatePosted = dateTime ?? DateTime.UtcNow;
    }

    [Key]
    public string ThreadId { get; set; }
    public string Title { get; set; }
    public string UserId { get; set; }
    public DateTime DatePosted { get; set; }
    public string ForumId { get; set; }

    public User User { get; set; } = null!;
    public Forum Forum { get; set; } = null!;
    public List<ForumReply> Replies { get; set; } = null!;
}