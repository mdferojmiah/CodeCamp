using learning_redis.Dtos;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace learning_redis.Controllers;

[ApiController]
[Route("pubsub-example")]
public class PubSubController(IConnectionMultiplexer redis) : ControllerBase
{
    private const string Channel = "system-notifier";

    [HttpPost]
    public IActionResult Notify(PubsubExampleRequestDto request)
    {
        var subcriber = redis.GetSubscriber();
        subcriber.Publish(RedisChannel.Literal(Channel), request.Message);
        return Ok("Message published successfully!");
    }
}