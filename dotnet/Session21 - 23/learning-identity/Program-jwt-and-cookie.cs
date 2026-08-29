// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.IdentityModel.Tokens;

// var builder = WebApplication.CreateBuilder(args);

// var key = "secret-key-secrect-key-secret-key-12345";

// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//                 .AddCookie(options => 
//                 {
//                     options.Events.OnRedirectToLogin = context => 
//                     {
//                         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                         return Task.CompletedTask;
//                     };
//                     options.Events.OnRedirectToAccessDenied = context =>
//                     {
//                         context.Response.StatusCode = StatusCodes.Status403Forbidden;
//                         return Task.CompletedTask;
//                     };
//                 })
//                 .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//                 {
//                     options.TokenValidationParameters = new TokenValidationParameters
//                     {
//                         ValidateIssuer = false,
//                         ValidateAudience = false,
//                         ValidateLifetime = true,
//                         ValidateIssuerSigningKey = true,
//                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
//                     };
//                 });

// builder.Services.AddAuthorization();

// var app = builder.Build();

// app.UseAuthentication();
// app.UseAuthorization();

// app.Use((context, next) =>
// {
//     return next();
// });

// app.MapGet("/login-with-cookie", (string username, string password, HttpContext context) =>
// {
//     if(username != "codecamp" && password != "password")
//     {
//         return Results.Unauthorized();
//     }

//     var claims = new List<Claim>
//     {
//         new(ClaimTypes.Name, username),
//         new(ClaimTypes.Role, "admin"),
//         new("auth-type", "cookie")
//     };

//     var claimsIndentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//     var claimsPrincipal = new ClaimsPrincipal(claimsIndentity);
//     context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
    
//     return Results.Ok("Cookie Authentication success!");
// });

// app.MapGet("/login-with-jwt", (string username, string password) =>
// {
//     if(username != "codecamp" && password != "password")
//     {
//         return Results.Unauthorized();
//     }

//     var claims = new List<Claim>
//     {
//         new(ClaimTypes.Name, username),
//         new(ClaimTypes.Role, "admin"),
//         new("auth-type", "jwt")
//     };

//     var tokendescriptor = new SecurityTokenDescriptor()
//     {
//         Subject = new ClaimsIdentity(claims),
//         Expires = DateTime.UtcNow.AddMinutes(30),
//         SigningCredentials = new SigningCredentials(
//             new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
//             SecurityAlgorithms.HmacSha256Signature
//         )
//     };

//     var handler = new JwtSecurityTokenHandler();
//     return Results.Ok(new {jwt = handler.WriteToken(handler.CreateToken(tokendescriptor))});
// });

// app.MapGet("/secure", () =>
// {
//     return Results.Ok("auth using nothing");
// }).RequireAuthorization();

// app.MapGet("/secure-cookie", () =>
// {
//     return Results.Ok("auth using cookie only");
// }).RequireAuthorization(new AuthorizeAttribute{AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme});

// app.MapGet("/secure-jwt", () =>
// {
//     return Results.Ok("auth using jwt only.");
// }).RequireAuthorization(new AuthorizeAttribute{AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme});

// app.MapGet("/secure-both", () => 
// {
//     return Results.Ok("auth using jwt and cookie");
// }).RequireAuthorization(new AuthorizeAttribute{AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}, {CookieAuthenticationDefaults.AuthenticationScheme}"});

// app.Run();