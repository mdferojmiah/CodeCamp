using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using learning_identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var provider = context.RequestServices.GetRequiredService<IDataProtectionProvider>();
    var protector = provider.CreateProtector("Cookie.Protector");

    var authCookie = context.Request.Headers.Cookie
                        .FirstOrDefault(c => c!.StartsWith("codecamp"));

    if (string.IsNullOrWhiteSpace(authCookie))
    {
        await next();
        return;
    }

    var encryptedCookie = authCookie?.Split("=").Last();
    // var encryptedParts = encryptedCookie?.Split("-");
    // if(encryptedParts == null || encryptedParts.Length < 2)
    // {
    //     await next();
    //     return;
    // }
    // var encrptedSecret = encryptedParts?[0];
    // var initializedVector = encryptedParts?[1];
    // var payload = EncryptionManager.Decrypt(encrptedSecret!, initializedVector!);
    var payload = protector.Unprotect(encryptedCookie!);
    var parts = payload?.Split(":");
    var key = parts?[0];
    var value = parts?[1];

    if(key != "secret" || value != "123456789")
    {
        await next();
        return;
    }

    var claims = new List<Claim>
    {
        new(key!, value!)
    };

    var claimsIndentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    context.User = new ClaimsPrincipal(claimsIndentity);

    await next();
});

app.MapGet("/cookie-authorized", (HttpContext context) =>
{
    var claims = context.User.FindFirst("secret");

    if(claims == null || claims.Value != "123456789") return Results.Unauthorized();

    return Results.Ok("user is cookie authorized!");
});

app.MapGet("/login", (string username, string password, HttpContext context, IDataProtectionProvider provider) =>
{

    if(username != "feroj" || password != "password") return Results.Unauthorized();

    var secret = $"secret:123456789";

    //var (encrptedSecret, initializedVector) = EncryptionManager.Encrypt(secret);
    var protector = provider.CreateProtector("Cookie.Protector");
    var encrypt = protector.Protect(secret);
    context.Response.Headers["set-cookie"] = $"codecamp={encrypt}";

    //context.Response.Headers["set-cookie"] = $"codecamp={encrptedSecret}-{initializedVector}";

    return Results.Ok("Login successful");
});

app.Run();