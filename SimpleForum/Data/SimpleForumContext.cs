using Microsoft.EntityFrameworkCore;
using SimpleForum.Models;

namespace SimpleForum.Data;

public class SimpleForumContext : DbContext
{
    public SimpleForumContext(DbContextOptions<SimpleForumContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User");
    }
}