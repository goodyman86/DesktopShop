using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using DesktopShop.Infrastructure.Data;
using DesktopShop.Infrastructure.Identity;
using DesktopShop.Infrastructure.Repositories;
using DesktopShop.Domain.Interfaces;
using DesktopShop.Application.Interfaces;
using DesktopShop.Application.Services;
using DesktopShop.Application.Mappings;
using DesktopShop.Application.Validators;
using DesktopShop.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ----- Database -----
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----- Identity -----
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ----- CORS (cho phép Web MVC truy cập API) -----
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebProject", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ----- Repositories & UnitOfWork -----
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ----- Application Services -----
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// ----- AutoMapper -----
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ----- FluentValidation -----
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();

// ----- Controllers + Swagger -----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ----- Seed Data -----
using (var scope = app.Services.CreateScope())
{
    await IdentitySeed.SeedAsync(scope.ServiceProvider);
}

// ----- Middleware Pipeline -----
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowWebProject");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
