namespace Domain.Models
{
  using System;

  /// <summary>
  /// A client app registered to use Identity for SSO, identified by a client id and validated by a hashed secret and a single allowed redirect domain.
  /// </summary>
  public class ClientApp
  {
    private ClientApp()
    {
    }

    /// <summary>
    /// The ClientApp class.
    /// </summary>
    /// <param name="name">Display name of the client app.</param>
    /// <param name="clientSecretHash">Hash of the client secret, never the plaintext value.</param>
    /// <param name="redirectDomain">Domain redirect URIs must belong to.</param>
    /// <param name="createdBy">Id of the user who registered the client app.</param>
    public ClientApp(string name, string clientSecretHash, string redirectDomain, Guid createdBy)
    {
      this.ClientId = Guid.NewGuid();
      this.Name = name;
      this.ClientSecret = clientSecretHash;
      this.RedirectDomain = redirectDomain;
      this.CreatedBy = createdBy;
      this.CreatedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// System-generated identifier the client app uses to identify itself.
    /// </summary>
    public Guid ClientId { get; private set; }

    /// <summary>
    /// Display name of the client app.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Hash of the client secret.
    /// </summary>
    public string ClientSecret { get; private set; } = string.Empty;

    /// <summary>
    /// Domain redirect URIs must belong to. A redirect URI is allowed only if its host matches this domain.
    /// </summary>
    public string RedirectDomain { get; private set; } = string.Empty;

    /// <summary>
    /// Id of the user who registered the client app.
    /// </summary>
    public Guid CreatedBy { get; private set; }

    /// <summary>
    /// User who registered the client app.
    /// </summary>
    public User? CreatedByUser { get; private set; }

    /// <summary>
    /// Timestamp of registration.
    /// </summary>
    public DateTime CreatedOn { get; private set; }

    /// <summary>
    /// Updates the client app's display name and allowed redirect domain.
    /// </summary>
    /// <param name="name">New display name.</param>
    /// <param name="redirectDomain">New domain redirect URIs must belong to.</param>
    public void UpdateDetails(string name, string redirectDomain)
    {
      this.Name = name;
      this.RedirectDomain = redirectDomain;
    }
  }
}
