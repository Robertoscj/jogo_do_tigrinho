using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using TigrinhoGame.Domain.Entities;
using TigrinhoGame.Domain.Interfaces;
using TigrinhoGame.Infrastructure.Data;

namespace TigrinhoGame.Infrastructure.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DapperContext _context;

        public PlayerRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Player?> GetByIdAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Players WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Player>(sql, new { Id = id });
        }

        public async Task<Player?> GetByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Players WHERE Email = @Email";
            return await connection.QuerySingleOrDefaultAsync<Player>(sql, new { Email = email });
        }

        public async Task<Player?> GetByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Players WHERE Username = @Username";
            return await connection.QuerySingleOrDefaultAsync<Player>(sql, new { Username = username });
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Players";
            return await connection.QueryAsync<Player>(sql);
        }

        public async Task<bool> AddAsync(Player player)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                INSERT INTO Players (Id, Username, Email, PasswordHash, Balance, CreatedAt, LastLoginAt, IsActive)
                VALUES (@Id, @Username, @Email, @PasswordHash, @Balance, @CreatedAt, @LastLoginAt, @IsActive)";
            
            var result = await connection.ExecuteAsync(sql, player);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(Player player)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                UPDATE Players 
                SET Username = @Username,
                    Email = @Email,
                    Balance = @Balance,
                    LastLoginAt = @LastLoginAt,
                    IsActive = @IsActive
                WHERE Id = @Id";
            
            var result = await connection.ExecuteAsync(sql, player);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            const string sql = "DELETE FROM Players WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }

        public async Task<bool> UpdateBalanceAsync(Guid id, decimal amount)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                UPDATE Players 
                SET Balance = Balance + @Amount
                WHERE Id = @Id AND (Balance + @Amount) >= 0";
            
            var result = await connection.ExecuteAsync(sql, new { Id = id, Amount = amount });
            return result > 0;
        }

        public async Task<decimal> GetBalanceAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT Balance FROM Players WHERE Id = @Id";
            return await connection.ExecuteScalarAsync<decimal>(sql, new { Id = id });
        }
    }
} 