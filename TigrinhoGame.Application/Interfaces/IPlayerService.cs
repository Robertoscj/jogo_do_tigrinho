using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TigrinhoGame.Application.DTOs;

namespace TigrinhoGame.Application.Interfaces
{
    public interface IPlayerService
    {
        Task<PlayerDto> RegisterPlayerAsync(CreatePlayerDto createPlayerDto);
        Task<PlayerDto> LoginAsync(PlayerLoginDto loginDto);
        Task<PlayerDto> GetPlayerByIdAsync(Guid id);
        Task<PlayerBalanceDto> GetPlayerBalanceAsync(Guid playerId);
        Task<bool> UpdatePlayerAsync(Guid id, UpdatePlayerDto updatePlayerDto);
        Task<bool> DeactivatePlayerAsync(Guid id);
        Task<IEnumerable<SpinHistoryDto>> GetPlayerSpinHistoryAsync(Guid playerId, DateTime? startDate = null, DateTime? endDate = null);
        Task<bool> HasSufficientBalanceAsync(Guid playerId, decimal betAmount);
        Task<decimal> GetPlayerRTPAsync(Guid playerId);
    }
} 