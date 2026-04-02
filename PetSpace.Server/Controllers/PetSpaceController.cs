using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetSpace.Core.Domain;
using PetSpace.Core.Dto;

namespace PetSpace.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetSpaceController : Controller
    {
        private readonly UserManager<User> _userManager;

        public PetSpaceController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "OK" });
            }

            return BadRequest(result.Errors);
        }
    }

}
