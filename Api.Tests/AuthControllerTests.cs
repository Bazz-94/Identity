namespace Api.Tests
{
  using System.Net;
  using System.Net.Http;
  using System.Net.Http.Json;
  using System.Threading.Tasks;
  using Xunit;

  public class AuthControllerTests : IClassFixture<ApiWebApplicationFactory>
  {
    private readonly ApiWebApplicationFactory factory;

    public AuthControllerTests(ApiWebApplicationFactory factory)
    {
      this.factory = factory;
    }

    [Fact]
    public async Task Login_KnownEmail_ReturnsOkAndSetsCookie()
    {
      HttpClient client = this.factory.CreateClient();

      HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/login", new { Email = ApiWebApplicationFactory.KnownEmail }, TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Assert.True(response.Headers.Contains("Set-Cookie"));
    }

    [Fact]
    public async Task Login_UnknownEmail_ReturnsUnauthorized()
    {
      HttpClient client = this.factory.CreateClient();

      HttpResponseMessage response = await client.PostAsJsonAsync("/api/auth/login", new { Email = ApiWebApplicationFactory.UnknownEmail }, TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Logout_ReturnsOk()
    {
      HttpClient client = this.factory.CreateClient();

      HttpResponseMessage response = await client.PostAsync("/api/auth/logout", null, TestContext.Current.CancellationToken);

      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}
