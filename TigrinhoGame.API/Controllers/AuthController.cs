using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TigrinhoGame.Application.DTOs;
using TigrinhoGame.Application.Interfaces;

namespace TigrinhoGame.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public AuthController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<PlayerDto>> Register([FromBody] CreatePlayerDto createPlayerDto)
        {
            try
            {
                var result = await _playerService.RegisterPlayerAsync(createPlayerDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<PlayerDto>> Login([FromBody] PlayerLoginDto loginDto)
        {
            try
            {
                var result = await _playerService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 