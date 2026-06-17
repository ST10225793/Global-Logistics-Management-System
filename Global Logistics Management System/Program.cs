using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Point the BaseAddress to the root of the backend container service directly
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5001/api/";

// Fallback safety: If it contains '/api/', trim it for the HttpClient root assignment
// so that relative paths like "api/ClientsApi" resolve perfectly.
var clientRootUrl = apiBaseUrl.EndsWith("/api/")
    ? apiBaseUrl.Replace("/api/", "/")
    : apiBaseUrl;

// 2. Configure the Named and Default HttpClients to use the root service address
builder.Services.AddHttpClient(string.Empty, client =>
{
    client.BaseAddress = new Uri(clientRootUrl);
});

builder.Services.AddHttpClient<Global_Logistics_Management_System.Services.CurrencyService>(client =>
{
    client.BaseAddress = new Uri(clientRootUrl);
});

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