using System.Collections.Generic;
using System.Threading.Tasks;
using TigrinhoGame.Domain.Entities;

namespace TigrinhoGame.Domain.Interfaces
{
    public interface IPayLineRepository
    {
        Task<PayLine> GetByIdAsync(int id);
        Task<IEnumerable<PayLine>> GetAllAsync();
        Task<IEnumerable<PayLine>> GetActivePayLinesAsync();
        Task<bool> AddAsync(PayLine payLine);
        Task<bool> UpdateAsync(PayLine payLine);
        Task<bool> DeleteAsync(int id);
    }
} 