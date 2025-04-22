using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<ContentItem> ContentItems { get; set; }
    public DbSet<UserProgress> UserProgresses { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }
    // public DbSet<TextContent> TextContents { get; set; }
    // public DbSet<VideoContent> VideoContents { get; set; }
    // public DbSet<ImageContent> ImageContents { get; set; }
    // public DbSet<LinkContent> LinkContents { get; set; }
    
    public DbSet<TestQuestion> TestQuestions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}