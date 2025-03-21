using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using TigrinhoGame.Domain.Entities;
using TigrinhoGame.Domain.Interfaces;
using TigrinhoGame.Infrastructure.Data;

namespace TigrinhoGame.Infrastructure.Repositories
{
    public class SymbolRepository : ISymbolRepository
    {
        private readonly DapperContext _context;

        public SymbolRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Symbol> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Symbols WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Symbol>(sql, new { Id = id });
        }

        public async Task<Symbol> GetByCodeAsync(string code)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Symbols WHERE Code = @Code";
            return await connection.QuerySingleOrDefaultAsync<Symbol>(sql, new { Code = code });
        }

        public async Task<IEnumerable<Symbol>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Symbols";
            return await connection.QueryAsync<Symbol>(sql);
        }

        public async Task<IEnumerable<Symbol>> GetActiveSymbolsAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Symbols WHERE IsActive = 1";
            return await connection.QueryAsync<Symbol>(sql);
        }

        public async Task<bool> AddAsync(Symbol symbol)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                INSERT INTO Symbols (Name, Code, PayoutMultiplier3X, PayoutMultiplier4X, PayoutMultiplier5X, Weight, IsWild, IsScatter, IsActive)
                VALUES (@Name, @Code, @PayoutMultiplier3X, @PayoutMultiplier4X, @PayoutMultiplier5X, @Weight, @IsWild, @IsScatter, @IsActive)";
            
            var result = await connection.ExecuteAsync(sql, symbol);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(Symbol symbol)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                UPDATE Symbols 
                SET Name = @Name,
                    Code = @Code,
                    PayoutMultiplier3X = @PayoutMultiplier3X,
                    PayoutMultiplier4X = @PayoutMultiplier4X,
                    PayoutMultiplier5X = @PayoutMultiplier5X,
                    Weight = @Weight,
                    IsWild = @IsWild,
                    IsScatter = @IsScatter,
                    IsActive = @IsActive
                WHERE Id = @Id";
            
            var result = await connection.ExecuteAsync(sql, symbol);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            const string sql = "DELETE FROM Symbols WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }

        public async Task<IEnumerable<Symbol>> GetWildSymbolsAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Symbols WHERE IsWild = 1 AND IsActive = 1";
            return await connection.QueryAsync<Symbol>(sql);
        }

        public async Task<IEnumerable<Symbol>> GetScatterSymbolsAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM Symbols WHERE IsScatter = 1 AND IsActive = 1";
            return await connection.QueryAsync<Symbol>(sql);
        }
    }
} 