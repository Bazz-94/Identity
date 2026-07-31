namespace Infrastructure.Database
{
  using Domain.Models;
  using Microsoft.EntityFrameworkCore;

  public class ModelDbContext : DbContext
  {
    public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
  }
}
