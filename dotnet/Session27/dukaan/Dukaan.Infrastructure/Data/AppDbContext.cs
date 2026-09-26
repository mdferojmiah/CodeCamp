using Dukaan.Domain.Products;
using Dukaan.Domain.Tenants;
using Dukaan.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dukaan.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options): IdentityDbContext<Merchant, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Merchant>().ToTable("Merchants");

        builder.Entity<CategorizedProduct>().HasKey(x => new { x.CategoryId, x.ProductId });

        builder.Entity<Category>()
            .HasOne(x => x.ParentCategory)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategorizedProduct> CategorizedProducts { get; set; }
}