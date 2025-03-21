using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TigrinhoGame.Application.DTOs;
using TigrinhoGame.Application.Interfaces;

namespace TigrinhoGame.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;

        public GameController(IGameService gameService, IPlayerService playerService)
        {
            _gameService = gameService;
            _playerService = playerService;
        }

        [HttpPost("spin")]
        public async Task<ActionResult<SpinResultDto>> Spin([FromBody] SpinRequestDto request)
        {
            try
            {
                var playerId = GetPlayerIdFromToken();
                request.PlayerId = playerId;

                // Validate bet amount
                if (!await _gameService.ValidateBetAmountAsync(request.BetAmount))
                    return BadRequest(new { message = "Invalid bet amount" });

                // Check if player has sufficient balance
                if (!await _playerService.HasSufficientBalanceAsync(playerId, request.BetAmount))
                    return BadRequest(new { message = "Insufficient balance" });

                // Check if game is available
                if (!await _gameService.IsGameAvailableAsync())
                    return BadRequest(new { message = "Game is currently unavailable" });

                // Check for active free spins
                var freeSpins = await _gameService.GetActiveFreeSpinsAsync(request);
                request.IsFreeSpin = freeSpins > 0;

                var result = await _gameService.SpinAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("rtp")]
        public async Task<ActionResult<decimal>> GetCurrentRTP()
        {
            try
            {
                var result = await _gameService.GetCurrentRTPAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("bet-limits")]
        public async Task<ActionResult<object>> GetBetLimits()
        {
            try
            {
                var minBet = await _gameService.GetMinBetAsync();
                var maxBet = await _gameService.GetMaxBetAsync();
                return Ok(new { minBet, maxBet });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private Guid GetPlayerIdFromToken()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "PlayerId");
            if (claim == null)
                throw new UnauthorizedAccessException("Invalid token");

            return Guid.Parse(claim.Value);
        }
    }
} 