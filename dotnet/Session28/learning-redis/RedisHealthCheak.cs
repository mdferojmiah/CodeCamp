using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using HealthCheckContext = Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext;
using HealthCheckResult = Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult;

namespace learning_redis;

public class RedisHealthCheak(IConnectionMultiplexer redis) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var db = redis.GetDatabase();
            var pong = await db.PingAsync();

            return pong != TimeSpan.Zero
                ? HealthCheckResult.Healthy("Redis is reachable")
                : HealthCheckResult.Unhealthy("Reding ping failed!");
        }
        catch(Exception ex)
        {
            return HealthCheckResult.Unhealthy("Redis connection failed: ", ex );
        }
    }
}