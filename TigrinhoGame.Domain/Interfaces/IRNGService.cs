using System.Collections.Generic;
using System.Threading.Tasks;
using TigrinhoGame.Domain.Entities;

namespace TigrinhoGame.Domain.Interfaces
{
    public interface IRNGService
    {
        Task<string[,]> GenerateSpinMatrixAsync(decimal targetRTP);
        Task<(bool IsWinning, string SymbolCode, int SymbolCount)> IsWinningCombinationAsync(string[,] matrix, PayLine payLine);
        Task<List<WinningLine>> CalculateWinningLines(string[,] matrix, IEnumerable<PayLine> activePayLines, decimal betAmount);
        Task<decimal> CalculateRTPContribution(decimal betAmount, decimal winAmount);
        Task<(bool IsTriggered, int FreeSpins)> ShouldTriggerBonusAsync(string[,] matrix);
    }
} 