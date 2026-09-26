namespace Dukaan.Application.Features.Auth.Dtos;

public record LoginResponseDto(string Token, DateTime Expiration);