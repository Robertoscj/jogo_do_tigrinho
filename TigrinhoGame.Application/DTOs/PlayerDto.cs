using System;

namespace TigrinhoGame.Application.DTOs
{
    public class PlayerDto
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public class CreatePlayerDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class UpdatePlayerDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
    }

    public class PlayerLoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class PlayerBalanceDto
    {
        public Guid PlayerId { get; set; }
        public decimal Balance { get; set; }
        public decimal PendingWins { get; set; }
    }
} 