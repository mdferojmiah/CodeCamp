using learning_redis.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace learning_redis.Controllers;

[ApiController]
[Route("/sorted-set-example")]
public class SortedSetExampleController(IConnectionMultiplexer redis) : ControllerBase
{
    private readonly IDatabase _db = redis.GetDatabase();
    private const string Key = "product-ranking";

    [HttpPost]
    public async Task<IActionResult> AddToRecord(SortedSetExampleDto request)
    {
        var score = await _db.SortedSetIncrementAsync(Key, request.ProductId, 1);

        return Ok($"New Score: {score}");
    }

    [HttpGet]
    public async Task<IActionResult> Top()
    {
        var rank = await _db.SortedSetRangeByRankWithScoresAsync(Key, 0, 4, Order.Descending);

        var leaderboard = rank.Select(x => new
        {
            ProductId = x.Element.ToString(),
            Score = (long) x.Score
        });

        return Ok(leaderboard);
    }
}