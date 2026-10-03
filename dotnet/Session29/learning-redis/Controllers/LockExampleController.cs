using learning_redis.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace learning_redis.Controllers;

[ApiController]
[Route("lock-example")]
public class LockExampleController(IConnectionMultiplexer redis) : ControllerBase
{
    private readonly IDatabase _db = redis.GetDatabase();
    private const string StoreKey = "Store:Inventory";
    private const string LockKey = "Lock:Inventory";

    [HttpPost("seed")]
    public IActionResult Seed(LockExampleRequestDto request)
    {
        _db.StringSet(StoreKey, request.Count);
        return Ok("Seed successfull!");
    }

    [HttpGet("inventory")]
    public IActionResult GetInventory()
    {
        var raw = _db.StringGet(StoreKey);
        var stock = raw.HasValue ? (int)raw : 0;

        return Ok(stock);
    }

    [HttpPost("inventory/decrement")]
    public IActionResult Decrement()
    {
        var token = Guid.NewGuid().ToString();

        var acquired = _db.LockTake(LockKey, token, TimeSpan.FromSeconds(10));
        if (!acquired) return Conflict("Could not acquire lock!");

        try
        {
            var raw = _db.StringGet(StoreKey);
            var stock = raw.HasValue ? (int)raw : 0;

            if(stock <= 0)
            {
                return BadRequest("Product is out of stock");
            }

            _db.StringSet(StoreKey, stock - 1);
        }
        finally
        {
            Thread.Sleep(7000);
            _db.LockRelease(LockKey, token);
        }

        return Ok("Stock Decresed!");
    }
}