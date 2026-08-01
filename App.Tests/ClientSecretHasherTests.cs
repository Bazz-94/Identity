namespace App.Tests
{
  using App.Helpers;
  using Xunit;

  public class ClientSecretHasherTests
  {
    [Fact]
    public void Verify_KnownSecretAgainstItsHash_Succeeds()
    {
      ClientSecretHasher hasher = new ClientSecretHasher();
      string hash = hasher.Hash("correct-secret");

      Assert.True(hasher.Verify("correct-secret", hash));
    }

    [Fact]
    public void Verify_WrongSecretAgainstHash_Fails()
    {
      ClientSecretHasher hasher = new ClientSecretHasher();
      string hash = hasher.Hash("correct-secret");

      Assert.False(hasher.Verify("wrong-secret", hash));
    }
  }
}
