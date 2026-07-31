namespace Migrations
{
  using Microsoft.EntityFrameworkCore;
  using Models;

  public class ModelDbContext : DbContext
  {
    public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
  }
}
