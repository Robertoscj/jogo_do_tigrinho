using System;

namespace TigrinhoGame.Domain.Entities
{
    public class Symbol
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public decimal PayoutMultiplier { get; private set; }
        public int Weight { get; private set; }
        public bool IsWild { get; private set; }
        public bool IsScatter { get; private set; }
        public bool IsActive { get; private set; }

        protected Symbol() { } // For EF Core

        public Symbol(string code, string name, decimal payoutMultiplier, int weight, bool isWild = false, bool isScatter = false)
        {
            Id = Guid.NewGuid();
            Code = code;
            Name = name;
            PayoutMultiplier = payoutMultiplier;
            Weight = weight;
            IsWild = isWild;
            IsScatter = isScatter;
            IsActive = true;
        }

        public decimal GetMultiplier(int symbolCount)
        {
            return symbolCount switch
            {
                3 => PayoutMultiplier,
                4 => PayoutMultiplier,
                5 => PayoutMultiplier,
                _ => 0
            };
        }

        public void UpdatePayoutMultiplier(decimal multiplier)
        {
            PayoutMultiplier = multiplier;
        }

        public void UpdateWeight(int weight)
        {
            Weight = weight;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
} 