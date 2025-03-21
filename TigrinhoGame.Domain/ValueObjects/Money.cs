using System;

namespace TigrinhoGame.Domain.ValueObjects
{
    public class Money : IEquatable<Money>
    {
        public decimal Value { get; }

        public Money(decimal value)
        {
            if (value < 0)
                throw new ArgumentException("Money value cannot be negative");

            if (decimal.Round(value, 2) != value)
                throw new ArgumentException("Money value cannot have more than 2 decimal places");

            Value = value;
        }

        public static Money operator +(Money a, Money b)
        {
            return new Money(a.Value + b.Value);
        }

        public static Money operator -(Money a, Money b)
        {
            return new Money(a.Value - b.Value);
        }

        public static Money operator *(Money a, decimal multiplier)
        {
            return new Money(a.Value * multiplier);
        }

        public static bool operator >(Money a, Money b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(Money a, Money b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(Money a, Money b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(Money a, Money b)
        {
            return a.Value <= b.Value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Money other)
                return Equals(other);
            return false;
        }

        public bool Equals(Money? other)
        {
            if (other is null)
                return false;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString("C2");
        }
    }
} 