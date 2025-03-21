using System;
using System.Collections.Generic;

namespace TigrinhoGame.Domain.Entities
{
    public class PayLine
    {
        public Guid Id { get; private set; }
        public int LineNumber { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public List<(int Row, int Col)> Positions { get; private set; } = new List<(int Row, int Col)>();
        public decimal Multiplier { get; private set; }
        public bool IsActive { get; private set; }

        protected PayLine() { } // For EF Core

        public PayLine(int lineNumber, string name, List<(int Row, int Col)> positions, decimal multiplier)
        {
            Id = Guid.NewGuid();
            LineNumber = lineNumber;
            Name = name;
            Positions = positions;
            Multiplier = multiplier;
            IsActive = true;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void UpdateMultiplier(decimal multiplier)
        {
            Multiplier = multiplier;
        }

        public bool CheckWinningLine(string[,] matrix, out string symbolCode, out int symbolCount)
        {
            symbolCode = matrix[Positions[0].Row, Positions[0].Col];
            symbolCount = 1;

            for (int i = 1; i < Positions.Count; i++)
            {
                var currentSymbol = matrix[Positions[i].Row, Positions[i].Col];
                if (currentSymbol != symbolCode)
                    break;
                symbolCount++;
            }

            return symbolCount >= 3;
        }
    }
} 