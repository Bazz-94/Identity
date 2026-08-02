namespace Api.Tests
{
  using System.Net;
  using System.Net.Http;
  using System.Net.Http.Json;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Mvc.Testing;
  using Xunit;

  public class RoleAuthorizationTests : IClassFixture<ApiWebApplicationFactory>
  {
    private readonly ApiWebApplicationFactory factory;

    public RoleAuthorizationTests(ApiWebApplicationFactory factory)
    {
      this.factory = factory;
    }

    [Fact]
    public async Task Get_Unauthenticated_ReturnsUnauthorizedWithLoginLocation()
    {
      HttpClient client = this.factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

      HttpResponseMessage response = await client.GetAsync("/api/client-apps", TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
      Assert.Equal("/", response.Headers.Location!.AbsolutePath);
    }

    [Fact]
    public async Task Get_AuthenticatedWrongRole_ReturnsForbiddenWithAccessDeniedLocation()
    {
      HttpClient client = this.factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
      await client.PostAsJsonAsync("/api/auth/login", new { Email = ApiWebApplicationFactory.RestrictedEmail }, TestContext.Current.CancellationToken);

      HttpResponseMessage response = await client.GetAsync("/api/client-apps", TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
      Assert.Contains("/AccessDenied", response.Headers.Location!.ToString());
    }

    [Fact]
    public async Task Get_AuthenticatedAdmin_Succeeds()
    {
      HttpClient client = this.factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
      await client.PostAsJsonAsync("/api/auth/login", new { Email = ApiWebApplicationFactory.AdminEmail }, TestContext.Current.CancellationToken);

      HttpResponseMessage response = await client.GetAsync("/api/client-apps", TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_AuthenticatedOperator_Succeeds()
    {
      HttpClient client = this.factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
      await client.PostAsJsonAsync("/api/auth/login", new { Email = ApiWebApplicationFactory.KnownEmail }, TestContext.Current.CancellationToken);

      HttpResponseMessage response = await client.GetAsync("/api/client-apps", TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_AuthenticatedOperator_ReturnsForbidden()
    {
      HttpClient client = this.factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
      await client.PostAsJsonAsync("/api/auth/login", new { Email = ApiWebApplicationFactory.KnownEmail }, TestContext.Current.CancellationToken);

      HttpResponseMessage response = await client.PostAsJsonAsync("/api/client-apps", new { Name = "Blocked App", RedirectDomain = "blocked.com" }, TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
  }
}
