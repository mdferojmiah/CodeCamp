using learning_redis.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace learning_redis.Controllers;

[ApiController]
[Route("/list-example")]
public class ListExampleController(IConnectionMultiplexer redis) : ControllerBase
{
    private readonly IDatabase _db = redis.GetDatabase();

    [HttpPost]
    public async Task<IActionResult> Search(ListExampleRequestDto request)
    {
        var key = $"search-history:{request.UserId}";
        await _db.ListLeftPushAsync(key, request.Query);
        await _db.ListTrimAsync(key, 0, 9);

        return Ok("ack");
    }

    [HttpGet("/search/history")]
    public async Task<IActionResult> GetSearchHistory([FromQuery] string userId)
    {
        var key = $"search-history:{userId}";
        var result = await _db.ListRangeAsync(key, 0, 4);

        return Ok(result.Select(x => x.ToString()));
    }
}