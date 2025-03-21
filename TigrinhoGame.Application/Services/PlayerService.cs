using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TigrinhoGame.Application.DTOs;
using TigrinhoGame.Application.Interfaces;
using TigrinhoGame.Domain.Entities;
using TigrinhoGame.Domain.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace TigrinhoGame.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ISpinRepository _spinRepository;
        private readonly IMapper _mapper;

        public PlayerService(IPlayerRepository playerRepository, ISpinRepository spinRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _spinRepository = spinRepository;
            _mapper = mapper;
        }

        public async Task<PlayerDto> RegisterPlayerAsync(CreatePlayerDto createPlayerDto)
        {
            // Validate if email or username already exists
            var existingEmail = await _playerRepository.GetByEmailAsync(createPlayerDto.Email);
            if (existingEmail != null)
                throw new InvalidOperationException("Email already registered");

            var existingUsername = await _playerRepository.GetByUsernameAsync(createPlayerDto.Username);
            if (existingUsername != null)
                throw new InvalidOperationException("Username already taken");

            // Create new player
            var passwordHash = BC.HashPassword(createPlayerDto.Password);
            var player = new Player(createPlayerDto.Username, createPlayerDto.Email, passwordHash);

            // Save player
            await _playerRepository.AddAsync(player);

            return _mapper.Map<PlayerDto>(player);
        }

        public async Task<PlayerDto> LoginAsync(PlayerLoginDto loginDto)
        {
            var player = await _playerRepository.GetByEmailAsync(loginDto.Email);
            if (player == null)
                throw new InvalidOperationException("Invalid email or password");

            if (!BC.Verify(loginDto.Password, player.PasswordHash))
                throw new InvalidOperationException("Invalid email or password");

            if (!player.IsActive)
                throw new InvalidOperationException("Account is deactivated");

            player.UpdateLastLogin();
            await _playerRepository.UpdateAsync(player);

            return _mapper.Map<PlayerDto>(player);
        }

        public async Task<PlayerDto> GetPlayerByIdAsync(Guid id)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null)
                throw new InvalidOperationException("Player not found");

            return _mapper.Map<PlayerDto>(player);
        }

        public async Task<PlayerBalanceDto> GetPlayerBalanceAsync(Guid playerId)
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            if (player == null)
                throw new InvalidOperationException("Player not found");

            var pendingWins = await _spinRepository.GetPlayerTotalWinsAsync(playerId);

            return new PlayerBalanceDto
            {
                PlayerId = playerId,
                Balance = player.Balance,
                PendingWins = pendingWins
            };
        }

        public async Task<bool> UpdatePlayerAsync(Guid id, UpdatePlayerDto updatePlayerDto)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null)
                throw new InvalidOperationException("Player not found");

            // Check if username is being changed and if it's available
            if (updatePlayerDto.Username != player.Username)
            {
                var existingUsername = await _playerRepository.GetByUsernameAsync(updatePlayerDto.Username);
                if (existingUsername != null)
                    throw new InvalidOperationException("Username already taken");
            }

            // Check if email is being changed and if it's available
            if (updatePlayerDto.Email != player.Email)
            {
                var existingEmail = await _playerRepository.GetByEmailAsync(updatePlayerDto.Email);
                if (existingEmail != null)
                    throw new InvalidOperationException("Email already registered");
            }

            _mapper.Map(updatePlayerDto, player);
            return await _playerRepository.UpdateAsync(player);
        }

        public async Task<bool> DeactivatePlayerAsync(Guid id)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            if (player == null)
                throw new InvalidOperationException("Player not found");

            player.Deactivate();
            return await _playerRepository.UpdateAsync(player);
        }

        public async Task<IEnumerable<SpinHistoryDto>> GetPlayerSpinHistoryAsync(Guid playerId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var history = await _spinRepository.GetPlayerSpinHistoryAsync(
                playerId,
                startDate ?? DateTime.UtcNow.AddDays(-30),
                endDate ?? DateTime.UtcNow
            );

            return _mapper.Map<IEnumerable<SpinHistoryDto>>(history);
        }

        public async Task<bool> HasSufficientBalanceAsync(Guid playerId, decimal betAmount)
        {
            var balance = await _playerRepository.GetBalanceAsync(playerId);
            return balance >= betAmount;
        }

        public async Task<decimal> GetPlayerRTPAsync(Guid playerId)
        {
            var totalBets = await _spinRepository.GetPlayerTotalBetsAsync(playerId);
            if (totalBets == 0)
                return 0;

            var totalWins = await _spinRepository.GetPlayerTotalWinsAsync(playerId);
            return totalWins / totalBets;
        }
    }
} 