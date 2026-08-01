namespace App.Enums
{
  /// <summary>
  /// Outcome of validating a client id and redirect URI against the client app registry.
  /// </summary>
  public enum ClientValidationResult
  {
    /// <summary>
    /// The client id is registered and the redirect URI's host matches its registered redirect domain.
    /// </summary>
    Valid,

    /// <summary>
    /// No client app is registered with the given client id.
    /// </summary>
    UnknownClient,

    /// <summary>
    /// The redirect URI's host does not match the client app's registered redirect domain.
    /// </summary>
    RedirectUriNotAllowed,
  }
}
