using Microsoft.EntityFrameworkCore;
using SimpleForum.Models;

namespace SimpleForum.Data;

public class SimpleForumContext : DbContext
{
    public SimpleForumContext(DbContextOptions<SimpleForumContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ForumThread> Threads { get; set; } = null!;
    public DbSet<ForumReply> Replies { get; set; } = null!;
    public DbSet<Forum> Forums { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<ForumThread>().ToTable("Thread");
        modelBuilder.Entity<ForumReply>().ToTable("Reply");
        modelBuilder.Entity<Forum>().ToTable("Forum");
    }
}