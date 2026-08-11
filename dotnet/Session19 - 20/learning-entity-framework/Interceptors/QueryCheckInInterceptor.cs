using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace learning_entity_framework.Interceptors;

public class QueryCheckInInterceptor: DbCommandInterceptor
{
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Executing: ==>{command.CommandText}");
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        var commandParam = command.Parameters.Count;
        Console.WriteLine($"Executed: ==>{command.CommandText}");
        Console.WriteLine($"Param Count: {commandParam}");
        return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }
}