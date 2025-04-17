using Microsoft.EntityFrameworkCore;
using Backend.Entity;
namespace Backend;

public class BotDbContext : DbContext
{
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<TextContent> TextContents { get; set; }
    public DbSet<VideoContent> VideoContents { get; set; }
    public DbSet<ImageContent> ImageContents { get; set; }
    public DbSet<LinkContent> LinkContents { get; set; }
    public DbSet<UserProgress> UserProgresses { get; set; }
    public DbSet<UserActivity> UserActivities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=telegram_bot_db;Username=postgres;Password=12345;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<Course>().HasKey(c => c.CourseId);
        modelBuilder.Entity<Topic>().HasKey(t => t.TopicId);
        modelBuilder.Entity<ContentItem>().HasKey(c => c.ContentId);
        modelBuilder.Entity<UserProgress>().HasKey(u => u.ProgressId);
        modelBuilder.Entity<UserActivity>().HasKey(u => u.ActivityId);
        modelBuilder.Entity<ContentItem>()
            
            .HasDiscriminator<string>("ContentType")
            .HasValue<TextContent>("text")
            .HasValue<VideoContent>("video")
            .HasValue<ImageContent>("image")
            .HasValue<LinkContent>("link");
        
        modelBuilder.Entity<UserProgress>()
            .HasOne(up => up.User)
            .WithMany()
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserProgress>()
            .HasOne(up => up.Content)
            .WithMany()
            .HasForeignKey(up => up.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserProgress>()
            .HasIndex(up => new { up.UserId, up.ContentId })
            .IsUnique();
        

        modelBuilder.Entity<UserActivity>()
            .HasOne(ua => ua.User)
            .WithMany()
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Topic>()
            .Property(t => t.Order)
            .HasColumnName("order");

        modelBuilder.Entity<ContentItem>()
            .Property(ci => ci.Order)
            .HasColumnName("order");

       
    }
}