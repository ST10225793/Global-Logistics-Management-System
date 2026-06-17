using Microsoft.EntityFrameworkCore;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Direct native address connection to your running Kestrel API on port 7230
string clientRootUrl = "https://localhost:7230/";

// Create a handler that bypasses SSL certificate validation errors for local testing
var bypassSslHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
};

// 1. Configure the DEFAULT HttpClient used by your standard MVC Controllers
builder.Services.AddHttpClient(string.Empty, client =>
{
    client.BaseAddress = new Uri(clientRootUrl);
}).ConfigurePrimaryHttpMessageHandler(() => bypassSslHandler);

// 2. Configure your typed CurrencyService HttpClient
builder.Services.AddHttpClient<Global_Logistics_Management_System.Services.CurrencyService>(client =>
{
    client.BaseAddress = new Uri(clientRootUrl);
}).ConfigurePrimaryHttpMessageHandler(() => bypassSslHandler);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();