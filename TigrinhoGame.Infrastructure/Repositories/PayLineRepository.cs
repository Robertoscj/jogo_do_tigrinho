using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using TigrinhoGame.Domain.Entities;
using TigrinhoGame.Domain.Interfaces;
using TigrinhoGame.Infrastructure.Data;

namespace TigrinhoGame.Infrastructure.Repositories
{
    public class PayLineRepository : IPayLineRepository
    {
        private readonly DapperContext _context;

        public PayLineRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<PayLine> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM PayLines WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<PayLine>(sql, new { Id = id });
        }

        public async Task<IEnumerable<PayLine>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM PayLines";
            return await connection.QueryAsync<PayLine>(sql);
        }

        public async Task<IEnumerable<PayLine>> GetActivePayLinesAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT * FROM PayLines WHERE IsActive = 1";
            return await connection.QueryAsync<PayLine>(sql);
        }

        public async Task<bool> AddAsync(PayLine payLine)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                INSERT INTO PayLines (Name, Positions, IsActive)
                VALUES (@Name, @Positions, @IsActive)";
            
            var result = await connection.ExecuteAsync(sql, new
            {
                payLine.Name,
                Positions = string.Join(",", payLine.Positions),
                payLine.IsActive
            });
            return result > 0;
        }

        public async Task<bool> UpdateAsync(PayLine payLine)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                UPDATE PayLines 
                SET Name = @Name,
                    Positions = @Positions,
                    IsActive = @IsActive
                WHERE Id = @Id";
            
            var result = await connection.ExecuteAsync(sql, new
            {
                payLine.Id,
                payLine.Name,
                Positions = string.Join(",", payLine.Positions),
                payLine.IsActive
            });
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            const string sql = "DELETE FROM PayLines WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }
} 