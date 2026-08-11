using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace learning_entity_framework.Interceptors;

public class SavechangesTimingInterceptor: SaveChangesInterceptor
{
    private readonly Stopwatch _stopWatch = new Stopwatch();
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        _stopWatch.Restart();
        return base.SavingChanges(eventData, result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        _stopWatch.Stop();
        var elapsedMilisecounds = _stopWatch.ElapsedMilliseconds;
        Console.WriteLine($"\nTotal time needed  to save: {elapsedMilisecounds} ms\n");

        return base.SavedChanges(eventData, result);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        _stopWatch.Stop();
        Console.WriteLine($"\nSavechanges failed after {_stopWatch.ElapsedMilliseconds} ms\n");
        
        base.SaveChangesFailed(eventData);
    }
}