namespace App.Services
{
  using System;
  using System.Threading.Tasks;
  using App.Enums;
  using Domain.Models;
  using Infrastructure.Database;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// Validates client ids and redirect URIs against the client app registry.
  /// </summary>
  public class ClientRegistryService
  {
    private readonly ModelDbContext dbContext;

    public ClientRegistryService(ModelDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    /// <summary>
    /// Validates that a client id is registered and that the redirect URI's host matches its registered redirect domain.
    /// </summary>
    /// <param name="clientId">The client id to look up.</param>
    /// <param name="redirectUri">The redirect URI to validate.</param>
    /// <returns>The validation outcome.</returns>
    public async Task<ClientValidationResult> ValidateClientAsync(Guid clientId, string redirectUri)
    {
      ClientApp? clientApp = await this.dbContext.ClientApps.SingleOrDefaultAsync(clientApp => clientApp.ClientId == clientId);
      if (clientApp is null)
      {
        return ClientValidationResult.UnknownClient;
      }

      if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out Uri? parsedRedirectUri) ||
          !string.Equals(parsedRedirectUri.Host, clientApp.RedirectDomain, StringComparison.OrdinalIgnoreCase))
      {
        return ClientValidationResult.RedirectUriNotAllowed;
      }

      return ClientValidationResult.Valid;
    }
  }
}
