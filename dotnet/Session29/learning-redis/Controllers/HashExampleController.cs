using System.Globalization;
using learning_redis.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace learning_redis.Controllers;

[ApiController]
[Route("hash-example")]
public class HashExampleController(IConnectionMultiplexer redis) : ControllerBase
{
    private readonly IDatabase _db = redis.GetDatabase();

    [HttpPost]
    public async Task<IActionResult> Create(HashExampleRequestDto requestDto)
    {
        var key = $"user:{requestDto.Id}";

        await _db.HashSetAsync(key, [
            new HashEntry("name", requestDto.Name),
            new HashEntry("email", requestDto.Email),
            new HashEntry("lastLogin", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture))
        ]);

        await _db.KeyExpireAsync(key, TimeSpan.FromMinutes(5));
        return Ok("Cache creadted successfully");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var key = $"user:{id}";

        var result = await _db.HashGetAllAsync(key);

        return Ok(result.ToDictionary(
            x => x.Name.ToString(), 
            x => x.Value.ToString()));
    }
}