using Dukaan.Application.Dtos;

namespace Dukaan.Application.Interfaces;

public interface IUserService
{
    Task<bool> CreateMarchantAsync(MerchantDto merchant, string password);
}