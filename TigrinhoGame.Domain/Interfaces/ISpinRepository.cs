using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TigrinhoGame.Domain.Entities;

namespace TigrinhoGame.Domain.Interfaces
{
    public interface ISpinRepository
    {
        Task<Spin?> GetByIdAsync(Guid id);
        Task<IEnumerable<Spin>> GetByPlayerIdAsync(Guid playerId, int limit = 20);
        Task<bool> AddAsync(Spin spin);
        Task<decimal> GetPlayerTotalBetsAsync(Guid playerId);
        Task<decimal> GetPlayerTotalWinsAsync(Guid playerId);
        Task<decimal> GetTotalRTPAsync();
        Task<IEnumerable<Spin>> GetPlayerSpinHistoryAsync(Guid playerId, DateTime startDate, DateTime endDate);
    }
} 