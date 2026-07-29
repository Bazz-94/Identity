namespace App
{
  using Api;
  using Client;

  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services from the libraries to the container.
      builder.Services.AddApi();
      builder.Services.AddClient();

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      // Map the endpoints exposed by each library.
      app.MapApi();
      app.MapClient();

      app.Run();
    }
  }
}
