// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.IdentityModel.Tokens;

// var builder = WebApplication.CreateBuilder(args);

// var app = builder.Build();

// var key = "secret-key-secrect-key-secret-key-12345";

// app.MapGet("/secure", (HttpContext context) => 
// {
//     var authHeader = context.Request.Headers.Authorization.ToString();
    
//     if(string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer ")){
//         return Results.Unauthorized();
//     }

//     var token = authHeader.Replace("Bearer ", "");

//     var validationParameters = new TokenValidationParameters(){
//         ValidateIssuer = false,
//         ValidateAudience = false,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
//     };

//     var handler = new JwtSecurityTokenHandler();

//     try
//     {
//         var claimsPrincipal = handler.ValidateToken(token, validationParameters, out _);
//         var username = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
//         var role = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;

//         return Results.Ok(new
//         {
//             message = "Security token varified!",
//             username,
//             role
//         });
//     }
//     catch
//     {
//         return Results.Unauthorized();
//     }
// });

// app.MapGet("/login-using-jwt", (string username, string password) =>
// {
//     if(username != "codecamp" && password != "password"){
//         return Results.Unauthorized();
//     }

//     var claims = new List<Claim>{
//         new(ClaimTypes.Name, username),
//         new (ClaimTypes.Role, "admin")
//     };

//     var tokenDescription = new SecurityTokenDescriptor()
//     {
//         Subject = new ClaimsIdentity(claims),
//         Expires = DateTime.UtcNow.AddMinutes(30),
//         SigningCredentials = new SigningCredentials(
//             new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
//             SecurityAlgorithms.HmacSha256Signature
//         )
//     };

//     var handler = new JwtSecurityTokenHandler();
//     var token = handler.CreateToken(tokenDescription);
//     var jwt = handler.WriteToken(token);

//     return Results.Ok(new { token = jwt });
// });

// app.Run();