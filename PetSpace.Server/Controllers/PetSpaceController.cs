using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSpace.Core.Dto;
using PetSpace.Core.ServiceInterface;

using System.Security.Claims;


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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();

            var userGuid = Guid.Parse(userIdClaim);

            var profile = await _petSpaceService.GetUserProfileAsync(userGuid);

            if (profile == null) return NotFound("Profile not found.");

            return Ok(profile);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var userGuid = Guid.Parse(userIdClaim);
            var result = await _petSpaceService.UpdateUserProfileAsync(userGuid, dto);

            if (!result)
                return BadRequest("Failed to update profile");

            return Ok();
        }

        [Authorize]
        [HttpGet("pets")]
        public async Task<IActionResult> GetPets()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var userGuid = Guid.Parse(userIdClaim);
            var pets = await _petSpaceService.GetUserPetsAsync(userGuid);

            return Ok(pets);
        }

        [Authorize]
        [HttpPost("pets")]
        public async Task<IActionResult> AddPet([FromBody] PetDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var userGuid = Guid.Parse(userIdClaim);
            var result = await _petSpaceService.AddPetAsync(userGuid, dto);

            if (!result)
                return BadRequest("Failed to add pet.");

            return Ok();
        }

        [Authorize]
        [HttpDelete("pets/{petId}")]
        public async Task<IActionResult> DeletePet(Guid petId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var userGuid = Guid.Parse(userIdClaim);
            var result = await _petSpaceService.DeletePetAsync(userGuid, petId);

            if (!result)
                return BadRequest("Failed to delete pet.");

            return Ok();
        }
    }
}
