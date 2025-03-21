using System;
using System.Collections.Generic;

namespace TigrinhoGame.Application.DTOs
{
    public class SpinRequestDto
    {
        public Guid PlayerId { get; set; }
        public decimal BetAmount { get; set; }
        public bool IsFreeSpin { get; set; }
    }

    public class SpinResultDto
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public decimal BetAmount { get; set; }
        public decimal WinAmount { get; set; }
        public bool IsFreeSpin { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal RTPContribution { get; set; }
        public decimal PlayerBalanceAfter { get; set; }
        public required string[,] Matrix { get; set; }
        public required List<WinningLineDto> WinningLines { get; set; }
        public bool BonusTriggered { get; set; }
        public int? FreeSpinsAwarded { get; set; }
    }

    public class WinningLineDto
    {
        public int LineNumber { get; set; }
        public required string SymbolCode { get; set; }
        public int SymbolCount { get; set; }
        public decimal WinAmount { get; set; }
    }

    public class SpinHistoryDto
    {
        public Guid Id { get; set; }
        public decimal BetAmount { get; set; }
        public decimal WinAmount { get; set; }
        public bool IsFreeSpin { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal RTPContribution { get; set; }
    }
} 