namespace App.Services
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using App.Enums;
  using App.Helpers;
  using Domain.Models;
  using Infrastructure.Database;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// Manages the client app registry: validation and administrative CRUD.
  /// </summary>
  public class ClientRegistryService
  {
    private readonly ModelDbContext dbContext;
    private readonly ClientSecretHasher hasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientRegistryService"/> class.
    /// </summary>
    /// <param name="dbContext">Database context backing the client app registry.</param>
    /// <param name="hasher">Hasher used to hash/verify client secrets.</param>
    public ClientRegistryService(ModelDbContext dbContext, ClientSecretHasher hasher)
    {
      this.dbContext = dbContext;
      this.hasher = hasher;
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

    /// <summary>
    /// Registers a new client app, generating and hashing its secret.
    /// </summary>
    /// <param name="name">Display name of the client app.</param>
    /// <param name="redirectDomain">Domain redirect URIs must belong to.</param>
    /// <param name="createdBy">Id of the user registering the client app.</param>
    /// <returns>The created client app and its plaintext secret.</returns>
    public async Task<(ClientApp ClientApp, string PlaintextSecret)> CreateClientAppAsync(string name, string redirectDomain, Guid createdBy)
    {
      string plaintextSecret = Guid.NewGuid().ToString("N");
      ClientApp clientApp = new ClientApp(name, this.hasher.Hash(plaintextSecret), redirectDomain, createdBy);
      this.dbContext.ClientApps.Add(clientApp);
      await this.dbContext.SaveChangesAsync();
      return (clientApp, plaintextSecret);
    }

    /// <summary>
    /// Lists all registered client apps, including their creator.
    /// </summary>
    /// <returns>All registered client apps.</returns>
    public async Task<List<ClientApp>> GetClientAppsAsync()
    {
      return await this.dbContext.ClientApps.Include(clientApp => clientApp.CreatedByUser).ToListAsync();
    }

    /// <summary>
    /// Updates an existing client app's name and redirect domain.
    /// </summary>
    /// <param name="clientId">Id of the client app to update.</param>
    /// <param name="name">New display name.</param>
    /// <param name="redirectDomain">New domain redirect URIs must belong to.</param>
    /// <returns>True if the client app was found and updated; otherwise false.</returns>
    public async Task<bool> UpdateClientAppAsync(Guid clientId, string name, string redirectDomain)
    {
      ClientApp? clientApp = await this.dbContext.ClientApps.SingleOrDefaultAsync(clientApp => clientApp.ClientId == clientId);
      if (clientApp is null)
      {
        return false;
      }

      clientApp.UpdateDetails(name, redirectDomain);
      await this.dbContext.SaveChangesAsync();
      return true;
    }

    /// <summary>
    /// Deletes a registered client app.
    /// </summary>
    /// <param name="clientId">Id of the client app to delete.</param>
    /// <returns>True if the client app was found and deleted; otherwise false.</returns>
    public async Task<bool> DeleteClientAppAsync(Guid clientId)
    {
      ClientApp? clientApp = await this.dbContext.ClientApps.SingleOrDefaultAsync(clientApp => clientApp.ClientId == clientId);
      if (clientApp is null)
      {
        return false;
      }

      this.dbContext.ClientApps.Remove(clientApp);
      await this.dbContext.SaveChangesAsync();
      return true;
    }
  }
}
