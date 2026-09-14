using Dukaan.Domain.Tenants;
using Microsoft.AspNetCore.Identity;

namespace Dukaan.Infrastructure.Data.Entities;

public class Merchant:  IdentityUser<Guid>, ITenantEntity
{
    public Guid TenantId { get; set; }
    public DateTime RegisterAt { get; set; } = DateTime.UtcNow;
}