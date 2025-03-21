using System.Threading.Tasks;
using TigrinhoGame.Application.DTOs;

namespace TigrinhoGame.Application.Interfaces
{
    public interface IGameService
    {
        Task<SpinResultDto> SpinAsync(SpinRequestDto request);
        Task<decimal> GetCurrentRTPAsync();
        Task<bool> IsGameAvailableAsync();
        Task<decimal> GetMinBetAsync();
        Task<decimal> GetMaxBetAsync();
        Task<int> GetActiveFreeSpinsAsync(SpinRequestDto request);
        Task<bool> ValidateBetAmountAsync(decimal betAmount);
    }
} 