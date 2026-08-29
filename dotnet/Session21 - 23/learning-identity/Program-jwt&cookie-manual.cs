// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.DataProtection;
// using Microsoft.IdentityModel.Tokens;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDataProtection();

// var key = "secret-key-secrect-key-secret-key-12345";

// var app = builder.Build();

// app.Use(async (context, next) =>
// {
//     var endpoint = context.GetEndpoint();
//     if(endpoint == null)
//     {
//         await next();
//         return;
//     }

//     var metadata = endpoint.Metadata.GetMetadata<AuthMetaData>();
//     if(metadata == null)
//     {
//         await next();
//         return;
//     }

//     var schemes = new List<string>();
//     foreach(var scheme in metadata.Schemes)
//     {
//         var metadataSchemes = scheme.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
//         schemes.AddRange(metadataSchemes);
//     }

//     if(schemes.Count == 0)
//     {
//         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//         await context.Response.WriteAsync("Unauthorized: No authentication scheme specified!");
//         return;
//     }

//     context.User = new ClaimsPrincipal();

//     foreach(var scheme in schemes)
//     {
//         if(scheme.Equals("Cookie", StringComparison.OrdinalIgnoreCase))
//         {
//             var cookieHeader = context.Request.Headers.Cookie.ToString();
//             if(cookieHeader == null) continue;

//             var encryptedCookie = cookieHeader.Split(';', StringSplitOptions.RemoveEmptyEntries)
//                                     .Select(x => x.Trim())
//                                     .FirstOrDefault(x => x.StartsWith("codecamp="));
//             if(encryptedCookie == null) continue;

//             var provider = context.RequestServices.GetRequiredService<IDataProtectionProvider>();
//             var protector = provider.CreateProtector("codecamp-protector");

//             try
//             {
//                 var claims = new List<Claim>();

//                 var payload = protector.Unprotect(encryptedCookie["codecamp=".Length..]);
//                 var values = payload.Split(',', StringSplitOptions.RemoveEmptyEntries);
//                 foreach(var value in values)
//                 {
//                     var parts = value.Split(':', StringSplitOptions.RemoveEmptyEntries);
//                     if(parts.Length != 2) continue;

//                     claims.Add(new Claim(parts[0], parts[1]));
//                 }
//                 var claimsIndentity = new ClaimsIdentity(claims, "Cookies");

//                 context.User.AddIdentity(claimsIndentity);
//             }
//             catch
//             {
//                 //invalid cookies
//             }
//         }

//         if(scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
//         {
//             var authHeader = context.Request.Headers.Authorization.ToString();
//             if(string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
//             {
//                 continue;
//             }

//             var token = authHeader.Replace("Bearer ", "");
//             var tokenValidationParameters = new TokenValidationParameters()
//             {
//                 ValidateIssuer = false,
//                 ValidateAudience = false,
//                 ValidateLifetime = true,
//                 ValidateIssuerSigningKey = true,
//                 IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
//             };

//             var handler = new JwtSecurityTokenHandler();
//             try
//             {
//                var claimsPrincipal = handler.ValidateToken(token, tokenValidationParameters, out _); 
//                var claims = claimsPrincipal.Claims.ToList();
//                var claimsIndentity = new ClaimsIdentity(claims, "Bearer");
//                context.User.AddIdentity(claimsIndentity);
//             }
//             catch
//             {
//                 //invalid token
//             }
//         }
//     }

//     if (!context.User.Identities.Any())
//     {
//         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//         await context.Response.WriteAsync("Unauthorized: No valid authentication provided!");
//         return;
//     }

//     await next();
// });

// app.MapGet("/login-with-cookie", (string username, string password, HttpContext context, IDataProtectionProvider idp) =>
// {
//     if(username != "codecamp" && password != "password")
//     {
//         return Results.Unauthorized();
//     }

//     var protector = idp.CreateProtector("codecamp-protector");
//     var encrptedSecret = protector.Protect($"username:{username},role:admin");

//     context.Response.Headers["set-cookie"] = $"codecamp={encrptedSecret};HttpOnly;Path=/";

//     return Results.Ok("Cookie authentication successful");
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

//     var tokendescription = new SecurityTokenDescriptor
//     {
//         Subject = new ClaimsIdentity(claims),
//         Expires = DateTime.UtcNow.AddMinutes(30),
//         SigningCredentials = new SigningCredentials(
//             new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
//             SecurityAlgorithms.HmacSha256Signature
//         )
//     };

//     var handler = new JwtSecurityTokenHandler();
//     return Results.Ok(new {token = handler.WriteToken(handler.CreateToken(tokendescription))});
// });

// app.MapGet("/secure-jwt", () =>
// {
//     return Results.Ok("Secure auth with jwt");
// }).WithMetadata(new AuthMetaData("Bearer"));

// app.MapGet("/secure-cookie", () =>
// {
//     return Results.Ok("Secure auth with cookie");
// }).WithMetadata(new AuthMetaData("Cookie"));

// app.Run();

// public class AuthMetaData
// {
//     public string[] Schemes { get; }

//     public AuthMetaData(params string[] schemes)
//     {
//         Schemes = schemes;
//     }
// }
