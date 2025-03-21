using System;
using System.Collections.Generic;
using TigrinhoGame.Domain.ValueObjects;

namespace TigrinhoGame.Domain.Entities
{
    public class Spin
    {
        public Guid Id { get; private set; }
        public Guid PlayerId { get; private set; }
        public decimal BetAmount { get; private set; }
        public decimal WinAmount { get; private set; }
        public bool IsFreeSpin { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal RTPContribution { get; private set; }
        public decimal PlayerBalanceAfter { get; private set; }
        public string[,] Matrix { get; private set; } = new string[3,5];
        public List<WinningLine> WinningLines { get; private set; } = new List<WinningLine>();

        protected Spin() { } // For EF Core

        public Spin(Guid playerId, decimal betAmount, bool isFreeSpin)
        {
            Id = Guid.NewGuid();
            PlayerId = playerId;
            BetAmount = betAmount;
            IsFreeSpin = isFreeSpin;
            CreatedAt = DateTime.UtcNow;
            WinAmount = 0;
        }

        public void SetMatrix(string[,] matrix)
        {
            Matrix = matrix;
        }

        public void AddWinningLine(WinningLine winningLine)
        {
            WinningLines.Add(winningLine);
            WinAmount += winningLine.WinAmount;
        }

        public void SetFinalResults(decimal playerBalanceAfter, decimal rtpContribution)
        {
            PlayerBalanceAfter = playerBalanceAfter;
            RTPContribution = rtpContribution;
        }
    }

    public class WinningLine
    {
        public int LineNumber { get; private set; }
        public string SymbolCode { get; private set; }
        public int SymbolCount { get; private set; }
        public decimal Multiplier { get; private set; }
        public decimal WinAmount { get; private set; }

        public WinningLine(int lineNumber, string symbolCode, int symbolCount, decimal multiplier, decimal betAmount)
        {
            LineNumber = lineNumber;
            SymbolCode = symbolCode;
            SymbolCount = symbolCount;
            Multiplier = multiplier;
            WinAmount = betAmount * multiplier;
        }
    }
} 