namespace Api.Tests
{
  using System;
  using System.Net;
  using System.Net.Http;
  using System.Net.Http.Json;
  using System.Threading.Tasks;
  using Api.Controllers;
  using Xunit;

  public class ClientAppsControllerTests : IClassFixture<ApiWebApplicationFactory>
  {
    private readonly ApiWebApplicationFactory factory;

    public ClientAppsControllerTests(ApiWebApplicationFactory factory)
    {
      this.factory = factory;
    }

    private async Task<HttpClient> CreateAdminClientAsync()
    {
      HttpClient client = this.factory.CreateClient();
      await client.PostAsJsonAsync("/api/auth/login", new { Email = ApiWebApplicationFactory.AdminEmail }, TestContext.Current.CancellationToken);
      return client;
    }

    [Fact]
    public async Task Create_ValidRequest_ReturnsCreatedWithPlaintextSecret()
    {
      HttpClient client = await this.CreateAdminClientAsync();

      HttpResponseMessage response = await client.PostAsJsonAsync("/api/client-apps", new { Name = "My App", RedirectDomain = "myapp.com" }, TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.Created, response.StatusCode);
      ClientAppCreatedResponse? body = await response.Content.ReadFromJsonAsync<ClientAppCreatedResponse>(TestContext.Current.CancellationToken);
      Assert.NotNull(body);
      Assert.NotEqual(Guid.Empty, body.ClientId);
      Assert.False(string.IsNullOrEmpty(body.ClientSecret));
    }

    [Fact]
    public async Task Create_EmptyName_ReturnsBadRequest()
    {
      HttpClient client = await this.CreateAdminClientAsync();

      HttpResponseMessage response = await client.PostAsJsonAsync("/api/client-apps", new { Name = string.Empty, RedirectDomain = "myapp.com" }, TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task List_ReturnsCreatedClientApps()
    {
      HttpClient client = await this.CreateAdminClientAsync();
      await client.PostAsJsonAsync("/api/client-apps", new { Name = "Listed App", RedirectDomain = "listed.com" }, TestContext.Current.CancellationToken);

      HttpResponseMessage response = await client.GetAsync("/api/client-apps", TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      ClientAppResponse[]? body = await response.Content.ReadFromJsonAsync<ClientAppResponse[]>(TestContext.Current.CancellationToken);
      Assert.NotNull(body);
      Assert.Contains(body, clientApp => clientApp.Name == "Listed App" && clientApp.CreatedByUserName == "Admin User");
    }

    [Fact]
    public async Task Update_ExistingClientApp_ReturnsNoContentAndUpdatesFields()
    {
      HttpClient client = await this.CreateAdminClientAsync();
      HttpResponseMessage createResponse = await client.PostAsJsonAsync("/api/client-apps", new { Name = "Original", RedirectDomain = "original.com" }, TestContext.Current.CancellationToken);
      ClientAppCreatedResponse created = (await createResponse.Content.ReadFromJsonAsync<ClientAppCreatedResponse>(TestContext.Current.CancellationToken))!;

      HttpResponseMessage updateResponse = await client.PutAsJsonAsync($"/api/client-apps/{created.ClientId}", new { Name = "Renamed", RedirectDomain = "renamed.com" }, TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
      ClientAppResponse[] listed = (await (await client.GetAsync("/api/client-apps", TestContext.Current.CancellationToken)).Content.ReadFromJsonAsync<ClientAppResponse[]>(TestContext.Current.CancellationToken))!;
      Assert.Contains(listed, clientApp => clientApp.ClientId == created.ClientId && clientApp.Name == "Renamed" && clientApp.RedirectDomain == "renamed.com");
    }

    [Fact]
    public async Task Update_UnknownClientApp_ReturnsNotFound()
    {
      HttpClient client = await this.CreateAdminClientAsync();

      HttpResponseMessage response = await client.PutAsJsonAsync($"/api/client-apps/{Guid.NewGuid()}", new { Name = "Renamed", RedirectDomain = "renamed.com" }, TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingClientApp_ReturnsNoContentAndRemovesIt()
    {
      HttpClient client = await this.CreateAdminClientAsync();
      HttpResponseMessage createResponse = await client.PostAsJsonAsync("/api/client-apps", new { Name = "To Delete", RedirectDomain = "todelete.com" }, TestContext.Current.CancellationToken);
      ClientAppCreatedResponse created = (await createResponse.Content.ReadFromJsonAsync<ClientAppCreatedResponse>(TestContext.Current.CancellationToken))!;

      HttpResponseMessage deleteResponse = await client.DeleteAsync($"/api/client-apps/{created.ClientId}", TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
      ClientAppResponse[] listed = (await (await client.GetAsync("/api/client-apps", TestContext.Current.CancellationToken)).Content.ReadFromJsonAsync<ClientAppResponse[]>(TestContext.Current.CancellationToken))!;
      Assert.DoesNotContain(listed, clientApp => clientApp.ClientId == created.ClientId);
    }

    [Fact]
    public async Task Delete_UnknownClientApp_ReturnsNotFound()
    {
      HttpClient client = await this.CreateAdminClientAsync();

      HttpResponseMessage response = await client.DeleteAsync($"/api/client-apps/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
  }
}
