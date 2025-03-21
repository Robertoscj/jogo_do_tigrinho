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
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("balance")]
        public async Task<ActionResult<PlayerBalanceDto>> GetBalance()
        {
            try
            {
                var playerId = GetPlayerIdFromToken();
                var result = await _playerService.GetPlayerBalanceAsync(playerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<SpinHistoryDto>>> GetHistory(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var playerId = GetPlayerIdFromToken();
                var result = await _playerService.GetPlayerSpinHistoryAsync(playerId, startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("rtp")]
        public async Task<ActionResult<decimal>> GetPlayerRTP()
        {
            try
            {
                var playerId = GetPlayerIdFromToken();
                var result = await _playerService.GetPlayerRTPAsync(playerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<PlayerDto>> UpdatePlayer([FromBody] UpdatePlayerDto updatePlayerDto)
        {
            try
            {
                var playerId = GetPlayerIdFromToken();
                var result = await _playerService.UpdatePlayerAsync(playerId, updatePlayerDto);
                return Ok(result);
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