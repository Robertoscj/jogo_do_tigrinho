using System.Collections.Generic;
using System.Threading.Tasks;
using TigrinhoGame.Domain.Entities;

namespace TigrinhoGame.Domain.Interfaces
{
    public interface ISymbolRepository
    {
        Task<Symbol> GetByIdAsync(int id);
        Task<Symbol> GetByCodeAsync(string code);
        Task<IEnumerable<Symbol>> GetAllAsync();
        Task<IEnumerable<Symbol>> GetActiveSymbolsAsync();
        Task<bool> AddAsync(Symbol symbol);
        Task<bool> UpdateAsync(Symbol symbol);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Symbol>> GetWildSymbolsAsync();
        Task<IEnumerable<Symbol>> GetScatterSymbolsAsync();
    }
} 