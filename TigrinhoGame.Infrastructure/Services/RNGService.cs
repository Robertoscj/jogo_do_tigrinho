using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TigrinhoGame.Domain.Entities;
using TigrinhoGame.Domain.Interfaces;
using TigrinhoGame.Domain.ValueObjects;

namespace TigrinhoGame.Infrastructure.Services
{
    public class RNGService : IRNGService
    {
        private readonly ISymbolRepository _symbolRepository;
        private readonly Random _random;

        public RNGService(ISymbolRepository symbolRepository)
        {
            _symbolRepository = symbolRepository;
            _random = new Random();
        }

        public async Task<string[,]> GenerateSpinMatrixAsync(decimal targetRTP)
        {
            var symbols = (await _symbolRepository.GetActiveSymbolsAsync()).ToList();
            var matrix = new string[5,3];
            var totalWeight = symbols.Sum(s => s.Weight);

           
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                  
                    var randomValue = _random.Next(1, totalWeight + 1);
                    var accumulatedWeight = 0;
                    
                    foreach (var symbol in symbols)
                    {
                        accumulatedWeight += symbol.Weight;
                        if (randomValue <= accumulatedWeight)
                        {
                            matrix[i,j] = symbol.Code;
                            break;
                        }
                    }
                }
            }

            return matrix;
        }

        public async Task<(bool IsWinning, string SymbolCode, int SymbolCount)> IsWinningCombinationAsync(string[,] matrix, PayLine payLine)
        {
            string symbolCode;
            int symbolCount;
            var isWinning = payLine.CheckWinningLine(matrix, out symbolCode, out symbolCount);
            return await Task.FromResult((isWinning, symbolCode, symbolCount));
        }

        public async Task<List<WinningLine>> CalculateWinningLines(string[,] matrix, IEnumerable<PayLine> activePayLines, decimal betAmount)
        {
            var winningLines = new List<WinningLine>();
            var symbols = await _symbolRepository.GetActiveSymbolsAsync();
            var wildSymbols = await _symbolRepository.GetWildSymbolsAsync();
            var wildCodes = wildSymbols.Select(s => s.Code).ToHashSet();

            foreach (var payLine in activePayLines)
            {
                var result = await IsWinningCombinationAsync(matrix, payLine);
                if (result.IsWinning)
                {
                    // Get the symbol and its multiplier
                    var symbol = symbols.First(s => s.Code == result.SymbolCode || (wildCodes.Contains(s.Code) && wildCodes.Contains(result.SymbolCode)));
                    var multiplier = symbol.GetMultiplier(result.SymbolCount);

                    if (multiplier > 0)
                    {
                        winningLines.Add(new WinningLine(
                            payLine.LineNumber,
                            result.SymbolCode,
                            result.SymbolCount,
                            multiplier,
                            betAmount
                        ));
                    }
                }
            }

            return winningLines;
        }

        public async Task<decimal> CalculateRTPContribution(decimal betAmount, decimal winAmount)
        {
            if (betAmount == 0)
                return await Task.FromResult(0m);

            return await Task.FromResult(winAmount / betAmount);
        }

        public async Task<(bool IsTriggered, int FreeSpins)> ShouldTriggerBonusAsync(string[,] matrix)
        {
            var scatterSymbols = await _symbolRepository.GetScatterSymbolsAsync();
            var scatterCodes = scatterSymbols.Select(s => s.Code).ToHashSet();
            
            var scatterCount = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (scatterCodes.Contains(matrix[i,j]))
                        scatterCount++;
                }
            }

            var freeSpins = scatterCount switch
            {
                3 => 10,
                4 => 15,
                5 => 20,
                _ => 0
            };

            return await Task.FromResult((freeSpins > 0, freeSpins));
        }
    }
} 