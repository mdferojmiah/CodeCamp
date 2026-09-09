using Microsoft.AspNetCore.Identity;

namespace dukaan.service.Entities;

public class Merchant:  IdentityUser<Guid>, ITenantEntity
{
    public Guid TenantId { get; set; }
    public DateTime RegisterAt { get; set; } = DateTime.UtcNow;
}