using DesktopShop.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ----- HttpClient → API Service -----
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
    ?? "http://localhost:5001";

builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// ----- Typed API Services -----
builder.Services.AddScoped<CategoryApiService>();
builder.Services.AddScoped<ProductApiService>();
builder.Services.AddScoped<OrderApiService>();
builder.Services.AddScoped<DashboardApiService>();

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
