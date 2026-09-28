using System.Text.Json;

namespace learning_redis.Dtos;

public record LearnRedisRequestDto(
    string Key, 
    JsonElement Value,
    int TtlSecounds);