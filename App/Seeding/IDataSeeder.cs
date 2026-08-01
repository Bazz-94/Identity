namespace App.Seeding
{
  using System.Threading.Tasks;
  using Infrastructure.Database;

  /// <summary>
  /// Seeds data for a specific set of entities. Whether and when an implementation runs is decided by its DI registration, not by this interface.
  /// </summary>
  public interface IDataSeeder
  {
    /// <summary>
    /// Seeds data if it doesn't already exist.
    /// </summary>
    /// <param name="dbContext">The database context to seed into.</param>
    Task SeedAsync(ModelDbContext dbContext);
  }
}
