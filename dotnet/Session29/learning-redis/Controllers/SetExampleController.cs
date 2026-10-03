using learning_redis.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace learning_redis.Controllers;

[ApiController]
[Route("set-example")]
public class SetExampleController(IConnectionMultiplexer redis) : ControllerBase
{
    private readonly IDatabase _db = redis.GetDatabase();
    private static readonly string TodayKey = $"visitors:{DateTime.UtcNow:yyyy-M-d}";

    [HttpPost]
    public async Task<IActionResult> AddVisitor(SetExampleRequestDto requestDto)
    {
        await _db.SetAddAsync(TodayKey, requestDto.Ip);

        return Ok("ack");
    }

    [HttpGet("/visitor/count")]
    public async Task<IActionResult> CountVistior()
    {
        var result = await _db.SetLengthAsync(TodayKey);

        return Ok(result);
    }
}