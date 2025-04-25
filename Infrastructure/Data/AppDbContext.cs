using Core.Entity;
using Core.Entity.AnyContent;
using Core.Entity.Progress;
using Core.Entity.Test;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Block> Blocks { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }
    
    public DbSet<ContentItem> ContentItems { get; set; }
    public DbSet<BookContent> BookContents { get; set; }
    public DbSet<AudioContent> AudioContents { get; set; }
    public DbSet<WordFileContent> WordFileContents { get; set; }
    public DbSet<ImageContent> ImageContents { get; set; }
    
    public DbSet<FinalTest> FinalTests { get; set; }
    public DbSet<TestQuestion> TestQuestions { get; set; }
    public DbSet<TestOption> TestOptions { get; set; }
    
    public DbSet<UserProgress> UserProgresses { get; set; }
    public DbSet<TopicProgress> TopicProgresses { get; set; }
    public DbSet<FinalTestProgress> FinalTestProgresses { get; set; }
    public DbSet<BlockCompletionProgress> BlockCompletionProgresses { get; set; }
    
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}