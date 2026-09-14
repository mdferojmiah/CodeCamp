namespace Dukaan.Domain.Tenants;

public interface ITenantEntity
{
    Guid TenantId { get; set; }
}