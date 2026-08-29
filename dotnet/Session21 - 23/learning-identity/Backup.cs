// using System.Security.Claims;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authentication.Cookies;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
// .AddCookie(options =>
// {
//     options.Cookie.Name = "codecamp";
// });
// builder.Services.AddAuthorization();

// var app = builder.Build();


// app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();

// app.MapGet("/cookie-authorized", (HttpContext context) =>
// {
//     return Results.Ok("You are authenticated by cookie!");
// })
// .RequireAuthorization();

// app.MapGet("/login", async (string username, string password, HttpContext context) =>
// {
//     if(username != "username" || password != "password")
//     {
//         return Results.Unauthorized();
//     }

//     var claims = new List<Claim>
//     {
//         new Claim("username", username),
//         new Claim("batch", "codecamp-4")
//     };

//     var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//     var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);

//     await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrinciple);
//     return Results.Ok("Login successful");
// });

// app.Run();


