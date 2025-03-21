using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using TigrinhoGame.Domain.Entities;
using TigrinhoGame.Domain.Interfaces;
using TigrinhoGame.Infrastructure.Data;

namespace TigrinhoGame.Infrastructure.Repositories
{
    public class SpinRepository : ISpinRepository
    {
        private readonly DapperContext _context;

        public SpinRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Spin?> GetByIdAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                SELECT * FROM Spins WHERE Id = @Id;
                SELECT * FROM WinningLines WHERE SpinId = @Id;";

            using var multi = await connection.QueryMultipleAsync(sql, new { Id = id });
            var spin = await multi.ReadSingleOrDefaultAsync<Spin>();
            if (spin != null)
            {
                var winningLines = await multi.ReadAsync<WinningLine>();
                foreach (var line in winningLines)
                {
                    spin.AddWinningLine(line);
                }
            }
            return spin;
        }

        public async Task<IEnumerable<Spin>> GetByPlayerIdAsync(Guid playerId, int limit = 20)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                SELECT TOP (@Limit) * FROM Spins 
                WHERE PlayerId = @PlayerId 
                ORDER BY CreatedAt DESC";

            return await connection.QueryAsync<Spin>(sql, new { PlayerId = playerId, Limit = limit });
        }

        public async Task<bool> AddAsync(Spin spin)
        {
            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Insert Spin
                const string spinSql = @"
                    INSERT INTO Spins (Id, PlayerId, BetAmount, WinAmount, Matrix, CreatedAt, IsFreeSpin, PlayerBalanceAfter, RTPContribution)
                    VALUES (@Id, @PlayerId, @BetAmount, @WinAmount, @Matrix, @CreatedAt, @IsFreeSpin, @PlayerBalanceAfter, @RTPContribution)";

                await connection.ExecuteAsync(spinSql, spin, transaction);

                // Insert WinningLines
                if (spin.WinningLines?.Count > 0)
                {
                    const string lineSql = @"
                        INSERT INTO WinningLines (SpinId, LineNumber, SymbolCode, SymbolCount, Multiplier, WinAmount)
                        VALUES (@SpinId, @LineNumber, @SymbolCode, @SymbolCount, @Multiplier, @WinAmount)";

                    foreach (var line in spin.WinningLines)
                    {
                        var parameters = new
                        {
                            SpinId = spin.Id,
                            line.LineNumber,
                            line.SymbolCode,
                            line.SymbolCount,
                            line.Multiplier,
                            line.WinAmount
                        };
                        await connection.ExecuteAsync(lineSql, parameters, transaction);
                    }
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<decimal> GetPlayerTotalBetsAsync(Guid playerId)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT COALESCE(SUM(BetAmount), 0) FROM Spins WHERE PlayerId = @PlayerId";
            return await connection.ExecuteScalarAsync<decimal>(sql, new { PlayerId = playerId });
        }

        public async Task<decimal> GetPlayerTotalWinsAsync(Guid playerId)
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT COALESCE(SUM(WinAmount), 0) FROM Spins WHERE PlayerId = @PlayerId";
            return await connection.ExecuteScalarAsync<decimal>(sql, new { PlayerId = playerId });
        }

        public async Task<decimal> GetTotalRTPAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                SELECT 
                    CASE 
                        WHEN SUM(BetAmount) = 0 THEN 0 
                        ELSE CAST(SUM(WinAmount) AS decimal(18,2)) / SUM(BetAmount) 
                    END 
                FROM Spins";
            return await connection.ExecuteScalarAsync<decimal>(sql);
        }

        public async Task<IEnumerable<Spin>> GetPlayerSpinHistoryAsync(Guid playerId, DateTime startDate, DateTime endDate)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                SELECT * FROM Spins 
                WHERE PlayerId = @PlayerId 
                AND CreatedAt BETWEEN @StartDate AND @EndDate
                ORDER BY CreatedAt DESC";

            return await connection.QueryAsync<Spin>(sql, new { PlayerId = playerId, StartDate = startDate, EndDate = endDate });
        }
    }
} 