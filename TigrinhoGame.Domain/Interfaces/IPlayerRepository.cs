using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TigrinhoGame.Domain.Entities;

namespace TigrinhoGame.Domain.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player?> GetByIdAsync(Guid id);
        Task<Player?> GetByEmailAsync(string email);
        Task<Player?> GetByUsernameAsync(string username);
        Task<IEnumerable<Player>> GetAllAsync();
        Task<bool> AddAsync(Player player);
        Task<bool> UpdateAsync(Player player);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateBalanceAsync(Guid playerId, decimal amount);
        Task<decimal> GetBalanceAsync(Guid playerId);
    }
} 