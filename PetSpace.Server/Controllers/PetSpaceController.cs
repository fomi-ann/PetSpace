using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetSpace.Core.Domain;
using PetSpace.Core.Dto;
using PetSpace.Core.ServiceInterface;
using PetSpace.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetSpace.Server.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetSpaceController : ControllerBase
    {
        private readonly IPetSpaceServices _petSpaceService;

        public PetSpaceController(
            IPetSpaceServices petSpaceService
            )
        {
            _petSpaceService = petSpaceService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            var result = await _petSpaceService.RegisterUserAsync(model);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _petSpaceService.LoginAsync(model);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                return Unauthorized();
            }

            var profile = await _petSpaceService.GetUserProfileAsync(userId, role);

            if (profile == null) return NotFound("Profile not found");

            return Ok(profile);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _petSpaceService.UpdateUserProfileAsync(userId, dto);

            if (!result)
                return BadRequest("Failed to update profile");

            return Ok();
        }
    }
}
