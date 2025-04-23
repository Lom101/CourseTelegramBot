using Core.Entity;
using Core.Entity.AnyContent;
using Core.Entity.Test;
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
    
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestQuestion> TestQuestions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}