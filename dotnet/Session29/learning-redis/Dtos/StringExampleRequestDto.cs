using System.Text.Json;

namespace learning_redis.Dtos;

public record StringExampleRequestDto(
    string Key, 
    JsonElement Value,
    int TtlSecounds);