using Dukaan.Domain.Tenants;

namespace Dukaan.Domain.Products;

public class CategorizedProduct : ITenantEntity
{
    public Guid ProductId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid TenantId { get; set; }

    public virtual Product Product { get; set; }
    public virtual Category Category { get; set; }
}