using ClimateBot.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClimateBot.Services;
using ClimateBot.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register your configuration singleton
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Register your NewsService related configurations
builder.Services.AddSingleton<INewsServiceFactory, NewsServiceFactory>();
builder.Services.AddHttpClient<INewsService, NewsService>((serviceProvider, httpClient) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    httpClient.BaseAddress = new Uri($"{configuration["NewsAPI:BaseUrl"]}{configuration["NewsAPI:ApiKey"]}");
});

// Register the ClimateService
// DESIGN PATTERN: Scoped or Singleton based on your use case
builder.Services.AddScoped<IClimateService, ClimateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
