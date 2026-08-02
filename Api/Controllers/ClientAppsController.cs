namespace Api.Controllers
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using System.Security.Claims;
  using System.Threading.Tasks;
  using App.Services;
  using Domain.Models;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;

  /// <summary>
  /// Administrative CRUD endpoints for the client app registry.
  /// </summary>
  [ApiController]
  [Route("api/client-apps")]
  [Authorize(Roles = "Admin,Operator")]
  public class ClientAppsController : ControllerBase
  {
    private readonly ClientRegistryService clientRegistryService;

    public ClientAppsController(ClientRegistryService clientRegistryService)
    {
      this.clientRegistryService = clientRegistryService;
    }

    /// <summary>
    /// Registers a new client app, returning its plaintext secret once.
    /// </summary>
    /// <param name="request">The client app's name and allowed redirect domain.</param>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateClientAppRequest request)
    {
      (ClientApp clientApp, string plaintextSecret) = await this.clientRegistryService.CreateClientAppAsync(
        request.Name,
        request.RedirectDomain,
        Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!));

      return this.CreatedAtAction(
        nameof(this.List),
        new ClientAppCreatedResponse(clientApp.ClientId, clientApp.Name, clientApp.RedirectDomain, plaintextSecret));
    }

    /// <summary>
    /// Lists all registered client apps.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List()
    {
      return this.Ok((await this.clientRegistryService.GetClientAppsAsync()).Select(clientApp => new ClientAppResponse(
        clientApp.ClientId,
        clientApp.Name,
        clientApp.RedirectDomain,
        clientApp.CreatedOn,
        clientApp.CreatedByUser?.UserName ?? string.Empty)));
    }

    /// <summary>
    /// Updates an existing client app's name and redirect domain.
    /// </summary>
    /// <param name="clientId">Id of the client app to update.</param>
    /// <param name="request">The client app's new name and allowed redirect domain.</param>
    [HttpPut("{clientId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid clientId, [FromBody] UpdateClientAppRequest request)
    {
      return await this.clientRegistryService.UpdateClientAppAsync(clientId, request.Name, request.RedirectDomain) ? this.NoContent() : this.NotFound();
    }

    /// <summary>
    /// Deletes a registered client app.
    /// </summary>
    /// <param name="clientId">Id of the client app to delete.</param>
    [HttpDelete("{clientId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid clientId)
    {
      return await this.clientRegistryService.DeleteClientAppAsync(clientId) ? this.NoContent() : this.NotFound();
    }
  }

  /// <summary>
  /// Request body for registering a client app.
  /// </summary>
  /// <param name="Name">Display name of the client app.</param>
  /// <param name="RedirectDomain">Domain redirect URIs must belong to.</param>
  public record CreateClientAppRequest([Required, MinLength(1)] string Name, [Required, MinLength(1)] string RedirectDomain);

  /// <summary>
  /// Request body for updating a client app.
  /// </summary>
  /// <param name="Name">New display name of the client app.</param>
  /// <param name="RedirectDomain">New domain redirect URIs must belong to.</param>
  public record UpdateClientAppRequest([Required, MinLength(1)] string Name, [Required, MinLength(1)] string RedirectDomain);

  /// <summary>
  /// Response returned after registering a client app, carrying its plaintext secret once.
  /// </summary>
  /// <param name="ClientId">System-generated client id.</param>
  /// <param name="Name">Display name of the client app.</param>
  /// <param name="RedirectDomain">Domain redirect URIs must belong to.</param>
  /// <param name="ClientSecret">Plaintext client secret, shown only in this response.</param>
  public record ClientAppCreatedResponse(Guid ClientId, string Name, string RedirectDomain, string ClientSecret);

  /// <summary>
  /// Response describing a registered client app, excluding its secret.
  /// </summary>
  /// <param name="ClientId">System-generated client id.</param>
  /// <param name="Name">Display name of the client app.</param>
  /// <param name="RedirectDomain">Domain redirect URIs must belong to.</param>
  /// <param name="CreatedOn">Timestamp of registration.</param>
  /// <param name="CreatedByUserName">Username of the user who registered the client app.</param>
  public record ClientAppResponse(Guid ClientId, string Name, string RedirectDomain, DateTime CreatedOn, string CreatedByUserName);
}
