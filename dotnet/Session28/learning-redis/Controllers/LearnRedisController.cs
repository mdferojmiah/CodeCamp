using System.Text.Json;
using learning_redis.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace learning_redis.Controllers;

[ApiController]
[Route("learn-redis")]
public class LearnRedisController(IConnectionMultiplexer redis) : ControllerBase
{
    [HttpPost]
    public IActionResult Set(LearnRedisRequestDto request)
    {
        var db = redis.GetDatabase();

        var json = JsonSerializer.Serialize(request.Value);
        db.StringSet(request.Key, json, TimeSpan.FromSeconds(request.TtlSecounds));
        
        return CreatedAtAction(nameof(Get), new {key = request.Key}, new
        {
            request.Key,
            request.TtlSecounds
        });
    }

    [HttpGet("{Key}")]
    public IActionResult Get([FromRoute] string key)
    {
        var db = redis.GetDatabase();

        var raw = db.StringGet(key);
        if (raw.IsNullOrEmpty)
        {
            return NoContent();
        }

        var value = JsonSerializer.Deserialize<JsonElement>(raw.ToString());
        return Ok(new { key, value });
    }
}