using System.Text;
using Dukaan.Application.Features.Auth.Services;
using Dukaan.Application.Features.Products.Services;
using Dukaan.Application.Features.Tenants.Services;
using Dukaan.Application.Interfaces;
using Dukaan.Infrastructure.Data;
using Dukaan.Infrastructure.Data.Entities;
using Dukaan.Infrastructure.Data.Services;
using Dukaan.Infrastructure.Interceptors;
using Dukaan.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<TenantInterceptor>();


builder.Services.AddDbContext<AppDbContext>((sp ,options) =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultString"))
        .AddInterceptors(sp.GetRequiredService<TenantInterceptor>());
});

builder.Services.AddIdentity<Merchant, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => 
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
