namespace App.Services
{
  using System.Threading.Tasks;
  using Domain.Models;
  using Infrastructure.Database;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// Looks up users by email for the login flow.
  /// </summary>
  public class AuthService
  {
    private readonly ModelDbContext dbContext;

    public AuthService(ModelDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    /// <summary>
    /// Finds the user registered with the given email.
    /// </summary>
    /// <param name="email">The email to look up.</param>
    /// <returns>The matching user, or null if none is registered.</returns>
    public async Task<User?> FindUserByEmailAsync(string email)
    {
      return await this.dbContext.Users.SingleOrDefaultAsync(user => user.Email == email);
    }
  }
}
