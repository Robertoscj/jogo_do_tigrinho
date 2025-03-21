using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using AutoMapper;
using TigrinhoGame.Application.DTOs;
using TigrinhoGame.Application.Interfaces;
using TigrinhoGame.Domain.Interfaces;
using TigrinhoGame.Domain.Entities;
using TigrinhoGame.Domain.ValueObjects;

namespace TigrinhoGame.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IRNGService _rngService;
        private readonly ISpinRepository _spinRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPayLineRepository _payLineRepository;
        private readonly IMapper _mapper;
        private readonly GameConfig _gameConfig;
        private readonly TransactionMonitoringService _transactionMonitoring;

        public GameService(
            IRNGService rngService,
            ISpinRepository spinRepository,
            IPlayerRepository playerRepository,
            IPayLineRepository payLineRepository,
            IMapper mapper,
            IOptions<GameConfig> gameConfig,
            TransactionMonitoringService transactionMonitoring)
        {
            _rngService = rngService;
            _spinRepository = spinRepository;
            _playerRepository = playerRepository;
            _payLineRepository = payLineRepository;
            _mapper = mapper;
            _gameConfig = gameConfig.Value;
            _transactionMonitoring = transactionMonitoring;
        }

        public async Task<SpinResultDto> SpinAsync(SpinRequestDto request)
        {
            // Get active pay lines
            var activePayLines = await _payLineRepository.GetActivePayLinesAsync();

            // Generate spin matrix
            var matrix = await _rngService.GenerateSpinMatrixAsync(_gameConfig.DefaultRTP);

            // Create spin entity
            var spin = new Spin(request.PlayerId, request.BetAmount, request.IsFreeSpin);
            spin.SetMatrix(matrix);

            // Calculate winning lines
            var winningLines = await _rngService.CalculateWinningLines(matrix, activePayLines, request.BetAmount);
            foreach (var line in winningLines)
            {
                spin.AddWinningLine(line);
            }

            // Check for bonus trigger
            var bonusResult = await _rngService.ShouldTriggerBonusAsync(matrix);

            // Update player balance
            decimal balanceChange = request.IsFreeSpin ? spin.WinAmount : spin.WinAmount - request.BetAmount;
            await _playerRepository.UpdateBalanceAsync(request.PlayerId, balanceChange);

            // Get updated balance
            var playerBalanceAfter = await _playerRepository.GetBalanceAsync(request.PlayerId);

            // Calculate RTP contribution
            var rtpContribution = await _rngService.CalculateRTPContribution(request.BetAmount, spin.WinAmount);
            spin.SetFinalResults(playerBalanceAfter, rtpContribution);

            // Save spin
            await _spinRepository.AddAsync(spin);

            // Map to DTO
            var result = _mapper.Map<SpinResultDto>(spin);
            result.BonusTriggered = bonusResult.IsTriggered;
            result.FreeSpinsAwarded = bonusResult.IsTriggered ? bonusResult.FreeSpins : null;

            // Monitor transaction for suspicious activity
            await _transactionMonitoring.MonitorSpinAsync(result);

            return result;
        }

        public async Task<decimal> GetCurrentRTPAsync()
        {
            return await _spinRepository.GetTotalRTPAsync();
        }

        public async Task<bool> IsGameAvailableAsync()
        {
            // Add any additional checks here (maintenance, etc.)
            return await Task.FromResult(true);
        }

        public async Task<decimal> GetMinBetAsync()
        {
            return await Task.FromResult(_gameConfig.MinBet);
        }

        public async Task<decimal> GetMaxBetAsync()
        {
            return await Task.FromResult(_gameConfig.MaxBet);
        }

        public async Task<int> GetActiveFreeSpinsAsync(SpinRequestDto request)
        {
            // This would typically check a player's free spins balance
            // For now, returning 0 as free spins tracking is not implemented
            return await Task.FromResult(0);
        }

        public async Task<bool> ValidateBetAmountAsync(decimal betAmount)
        {
            return await Task.FromResult(
                betAmount >= _gameConfig.MinBet && 
                betAmount <= _gameConfig.MaxBet
            );
        }
    }

    public class GameConfig
    {
        public decimal DefaultRTP { get; set; }
        public decimal MinBet { get; set; }
        public decimal MaxBet { get; set; }
        public decimal InitialBalance { get; set; }
    }
} 