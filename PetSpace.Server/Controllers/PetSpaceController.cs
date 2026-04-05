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
    }
}
