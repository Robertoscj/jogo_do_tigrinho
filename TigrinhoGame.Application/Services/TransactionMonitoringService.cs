using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TigrinhoGame.Application.DTOs;
using TigrinhoGame.Domain.Interfaces;

namespace TigrinhoGame.Application.Services
{
    public class TransactionMonitoringService
    {
        private readonly ILogger<TransactionMonitoringService> _logger;
        private readonly IPlayerRepository _playerRepository;
        private readonly ISpinRepository _spinRepository;
        private readonly ConcurrentDictionary<Guid, PlayerMonitoringData> _playerData;

        private const decimal SUSPICIOUS_WIN_RATE = 0.8m; // 80% win rate
        private const decimal SUSPICIOUS_RTP = 0.98m; // 98% RTP
        private const int MONITORING_WINDOW_MINUTES = 60;
        private const int MIN_TRANSACTIONS_FOR_MONITORING = 10;

        public TransactionMonitoringService(
            ILogger<TransactionMonitoringService> logger,
            IPlayerRepository playerRepository,
            ISpinRepository spinRepository)
        {
            _logger = logger;
            _playerRepository = playerRepository;
            _spinRepository = spinRepository;
            _playerData = new ConcurrentDictionary<Guid, PlayerMonitoringData>();
        }

        public async Task MonitorSpinAsync(SpinResultDto spinResult)
        {
            var playerData = _playerData.GetOrAdd(spinResult.PlayerId, _ => new PlayerMonitoringData());

            // Update player statistics
            playerData.TotalSpins++;
            playerData.TotalBet += spinResult.BetAmount;
            playerData.TotalWin += spinResult.WinAmount;

            if (spinResult.WinAmount > 0)
                playerData.WinningSpins++;

            // Remove old transactions
            var cutoffTime = DateTime.UtcNow.AddMinutes(-MONITORING_WINDOW_MINUTES);
            playerData.CleanupOldTransactions(cutoffTime);

            // Add new transaction
            playerData.AddTransaction(new TransactionData
            {
                Timestamp = DateTime.UtcNow,
                BetAmount = spinResult.BetAmount,
                WinAmount = spinResult.WinAmount
            });

            // Check for suspicious activity if we have enough data
            if (playerData.TotalSpins >= MIN_TRANSACTIONS_FOR_MONITORING)
            {
                await CheckForSuspiciousActivityAsync(spinResult.PlayerId, playerData);
            }
        }

        private async Task CheckForSuspiciousActivityAsync(Guid playerId, PlayerMonitoringData data)
        {
            var winRate = (decimal)data.WinningSpins / data.TotalSpins;
            var rtp = data.TotalWin / data.TotalBet;

            var suspiciousActivity = false;
            var reason = string.Empty;

            if (winRate > SUSPICIOUS_WIN_RATE)
            {
                suspiciousActivity = true;
                reason = $"High win rate detected: {winRate:P2}";
            }
            else if (rtp > SUSPICIOUS_RTP)
            {
                suspiciousActivity = true;
                reason = $"Abnormal RTP detected: {rtp:P2}";
            }

            if (suspiciousActivity)
            {
                _logger.LogWarning(
                    "Suspicious activity detected for player {PlayerId}. Reason: {Reason}. " +
                    "Stats: Spins={TotalSpins}, WinRate={WinRate}, RTP={RTP}",
                    playerId,
                    reason,
                    data.TotalSpins,
                    winRate,
                    rtp);

                // Get player details
                var player = await _playerRepository.GetByIdAsync(playerId);
                if (player != null)
                {
                    // You could implement additional actions here:
                    // - Temporarily suspend the player's account
                    // - Send notifications to administrators
                    // - Add the player to a watch list
                    // - Trigger a detailed audit
                }
            }
        }

        private class PlayerMonitoringData
        {
            public int TotalSpins { get; set; }
            public int WinningSpins { get; set; }
            public decimal TotalBet { get; set; }
            public decimal TotalWin { get; set; }
            private readonly ConcurrentQueue<TransactionData> _recentTransactions;

            public PlayerMonitoringData()
            {
                _recentTransactions = new ConcurrentQueue<TransactionData>();
            }

            public void AddTransaction(TransactionData transaction)
            {
                _recentTransactions.Enqueue(transaction);
            }

            public void CleanupOldTransactions(DateTime cutoffTime)
            {
                while (_recentTransactions.TryPeek(out var transaction) && 
                       transaction.Timestamp < cutoffTime)
                {
                    _recentTransactions.TryDequeue(out _);
                }
            }
        }

        private class TransactionData
        {
            public DateTime Timestamp { get; set; }
            public decimal BetAmount { get; set; }
            public decimal WinAmount { get; set; }
        }
    }
} 