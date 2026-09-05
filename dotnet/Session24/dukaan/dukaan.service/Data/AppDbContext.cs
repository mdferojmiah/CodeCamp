using dukaan.service.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dukaan.service.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options): IdentityDbContext<Merchant, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Merchant>().ToTable("Merchants");
    }

    public DbSet<Tenant> Tenants { get; set; }
}