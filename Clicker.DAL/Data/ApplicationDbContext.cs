using Clicker.DAL.Data.Configurations;
using Clicker.DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace Clicker.DAL.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserChannelSubscriptionTask> UserChannelSubscriptionTasks { get; set; }
    public DbSet<UserOfferSubscriptionTask> UserOfferSubscriptionTasks { get; set; }
    public DbSet<ChannelSubscriptionTask> ChannelSubscriptionTasks { get; set; }
    public DbSet<OfferSubscriptionTask> OfferSubscriptionTasks { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ChannelSubscriptionBaseEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OfferSubscriptionBaseEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserChannelSubscriptionTaskEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserOfferSubscriptionTaskEntityConfiguration());
       
    }
}

