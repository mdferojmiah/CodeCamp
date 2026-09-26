using Microsoft.AspNetCore.Http;

namespace Dukaan.Infrastructure.Data.Services;

public interface ITenantProvider
{
    Guid? GetTenantId();
}

public class TenantProvider(IHttpContextAccessor httpContextAccessor): ITenantProvider
{
    public Guid? GetTenantId()
    {
        var tenantIdClaim = httpContextAccessor.HttpContext?.User.FindFirst("tenant_id")?.Value;
        return Guid.TryParse(tenantIdClaim, out var tenantId) ? tenantId : null;
    }
}