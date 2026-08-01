namespace App.Helpers
{
  using Microsoft.AspNetCore.Identity;

  /// <summary>
  /// Hashes and verifies client app secrets, so the plaintext value is never persisted.
  /// </summary>
  public class ClientSecretHasher
  {
    private readonly PasswordHasher<ClientSecretHasher> hasher = new PasswordHasher<ClientSecretHasher>();

    /// <summary>
    /// Hashes a plaintext client secret.
    /// </summary>
    /// <param name="secret">The plaintext secret.</param>
    /// <returns>The hash to persist.</returns>
    public string Hash(string secret)
    {
      return this.hasher.HashPassword(this, secret);
    }

    /// <summary>
    /// Verifies a plaintext client secret against a stored hash.
    /// </summary>
    /// <param name="secret">The plaintext secret to verify.</param>
    /// <param name="hash">The stored hash to verify against.</param>
    /// <returns>True if the secret matches the hash.</returns>
    public bool Verify(string secret, string hash)
    {
      PasswordVerificationResult result = this.hasher.VerifyHashedPassword(this, hash, secret);
      return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
  }
}
