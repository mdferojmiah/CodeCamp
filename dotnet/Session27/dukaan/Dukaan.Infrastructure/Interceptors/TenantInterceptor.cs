using Dukaan.Domain.Tenants;
using Dukaan.Infrastructure.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Dukaan.Infrastructure.Interceptors;

public class TenantInterceptor(ITenantProvider tenantProvider): SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntries(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        UpdateEntries(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntries(DbContext? dbContext)
    {
        if (dbContext == null) return;

        var tenantId = tenantProvider.GetTenantId();

        foreach (var entry in dbContext.ChangeTracker.Entries<ITenantEntity>())
        {
            if(entry.State != EntityState.Added) continue;

            if(entry.Entity.TenantId == Guid.Empty)
            {
                entry.Entity.TenantId = tenantId ?? throw new Exception("TenatId is missing while creating tenant context entity");
            }
        }
    }
}
