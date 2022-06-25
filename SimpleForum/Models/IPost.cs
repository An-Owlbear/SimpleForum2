namespace SimpleForum.Models;

public interface IPost
{
    public string Content { get; set; }
    public DateTime DatePosted { get; set; }
    public User User { get; set; }
}