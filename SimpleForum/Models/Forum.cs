using System.ComponentModel.DataAnnotations;

namespace SimpleForum.Models;

public class Forum
{
    private Forum(string forumId, string name)
    {
        ForumId = forumId;
        Name = name;
    }

    public Forum(string name)
    {
        ForumId = new Guid().ToString();
        Name = name;
    }
    
    [Key]
    public string ForumId { get; set; }
    public string Name { get; set; }

    public List<ForumThread> Threads { get; set; } = null!;
}