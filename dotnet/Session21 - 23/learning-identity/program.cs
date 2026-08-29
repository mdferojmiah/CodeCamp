using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using learning_identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var key = "secret-key-secrect-key-secret-key-12345"u8.ToArray();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=5433;Database=identity_db;Username=postgres;Password=12345");
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequiredLength = 1;
}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "learning-identity-framework",
                        ValidAudience = "learning-identity-framework",
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin-policy", policy =>
    {
        policy.RequireClaim("org", "google");
        policy.RequireClaim(ClaimTypes.Role, "admin");
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/register", async (
    string email, 
    string password, 
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager) =>
{
    var user = new IdentityUser
    {
        UserName = email,
        Email = email
    };
    var result = await userManager.CreateAsync(user, password);

    var role = email switch
    {
        var e when e.StartsWith("feroj") => "admin",
        _ => "user"
    };

    var isRoleExists = await roleManager.RoleExistsAsync(role);
    if(!isRoleExists) await roleManager.CreateAsync(new IdentityRole(role));
    await userManager.AddToRoleAsync(user, role);

    var organization = email switch
    {
        var e when e.EndsWith("@ait.com") => "ait",
        var e when e.EndsWith("@optimizely.com") => "optimizely",
        var e when e.EndsWith("@fieldnation.com") => "fieldnation",
        var e when e.EndsWith("@gmail.com") => "google",
        _ => "unknown"
    };

    await userManager.AddClaimAsync(user, new Claim("org", organization));

    return !result.Succeeded ? Results.BadRequest(result.Errors) : Results.Ok("User Created with roles and cliams");
});

app.MapGet("/login", async (string email, string password, UserManager<IdentityUser> userManager) =>
{
    if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
    {
        return Results.Unauthorized();
    } 

    var user = await userManager.FindByEmailAsync(email);
    if(user == null) return Results.Unauthorized();

    var isValidPassword = await userManager.CheckPasswordAsync(user, password);
    if(!isValidPassword) return Results.Unauthorized();

    var userClaims = await userManager.GetClaimsAsync(user);
    var roles = await userManager.GetRolesAsync(user);

    //adding claims
    var allClaims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Email, email)
    };
    allClaims.AddRange(userClaims);
    foreach(var role in roles)
    {
        allClaims.Add(new Claim(ClaimTypes.Role, role));
    }

    var tokenDescription = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(allClaims),
        Expires = DateTime.UtcNow.AddMinutes(30),
        Issuer = "learning-identity-framework",
        Audience = "learning-identity-framework",
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        )
    };

    var handler = new JwtSecurityTokenHandler();
    var token = handler.CreateToken(tokenDescription);
    var jwtToken = handler.WriteToken(token);

    return Results.Ok(new {token = jwtToken});
});

app.MapGet("/ait-resources", () =>
{
    return Results.Ok("You are accessing AIT resources");
}).RequireAuthorization(policy => policy.RequireClaim("org", "ait"));

app.MapGet("/optimizely-resources", () =>
{
    return Results.Ok("You are accessing Optimizely resources");
}).RequireAuthorization(policy => policy.RequireClaim("org", "optimizely"));;

app.MapGet("/fieldnation-resources", () =>
{
    return Results.Ok("You are accessing FieldNation resources");
}).RequireAuthorization(policy => policy.RequireClaim("org", "fieldnation"));;

app.MapGet("/ait-admin-resources", () =>
{
    return Results.Ok("You are accessing AIT admin resources");
}).RequireAuthorization(policy => policy
    .RequireClaim("org", "ait")
    .RequireClaim(ClaimTypes.Role, "admin"));

app.MapGet("/admin-resources", () =>
{
    return Results.Ok("You are accessing admin resources");
}).RequireAuthorization("admin-policy");

app.Run();