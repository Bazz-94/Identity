namespace Infrastructure.Database
{
  using Domain.Models;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// EF Core database context for Identity's persisted models.
  /// </summary>
  public class ModelDbContext : DbContext
  {
    public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<ClientApp> ClientApps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ClientApp>()
        .HasKey(clientApp => clientApp.ClientId);

      modelBuilder.Entity<ClientApp>()
        .HasOne(clientApp => clientApp.CreatedByUser)
        .WithMany()
        .HasForeignKey(clientApp => clientApp.CreatedBy)
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
