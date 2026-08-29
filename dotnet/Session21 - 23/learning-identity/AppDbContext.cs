using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace learning_identity;

public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions): IdentityDbContext<IdentityUser>(dbContextOptions)
{
    
}