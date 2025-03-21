using System;

namespace TigrinhoGame.Domain.ValueObjects
{
    public class RTP : IEquatable<RTP>
    {
        public decimal Value { get; }
        public const decimal MinValue = 0.90M; // 90%
        public const decimal MaxValue = 0.97M; // 97%
        public const decimal DefaultValue = 0.96M; // 96%

        private RTP(decimal value)
        {
            if (value < MinValue || value > MaxValue)
                throw new ArgumentException($"RTP must be between {MinValue:P0} and {MaxValue:P0}");

            Value = value;
        }

        public static RTP Create(decimal value)
        {
            return new RTP(value);
        }

        public static RTP Default()
        {
            return new RTP(DefaultValue);
        }

        public decimal CalculateHouseEdge()
        {
            return 1 - Value;
        }

        public bool IsWithinTargetRange(decimal actualRTP, decimal tolerance = 0.01M)
        {
            return Math.Abs(actualRTP - Value) <= tolerance;
        }

        public override string ToString()
        {
            return $"{Value:P2}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is RTP other)
                return Equals(other);
            return false;
        }

        public bool Equals(RTP? other)
        {
            if (other is null)
                return false;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static RTP operator +(RTP a, RTP b)
        {
            return new RTP(a.Value + b.Value);
        }

        public static RTP operator -(RTP a, RTP b)
        {
            return new RTP(a.Value - b.Value);
        }

        public static RTP operator /(RTP a, int divisor)
        {
            return new RTP(a.Value / divisor);
        }

        public static bool operator >(RTP a, RTP b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(RTP a, RTP b)
        {
            return a.Value < b.Value;
        }
    }
} 