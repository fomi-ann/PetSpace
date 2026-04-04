using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSpace.Core.Domain;
using PetSpace.Core.Dto;
using PetSpace.Data;

namespace PetSpace.Server.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetSpaceController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly PetSpaceDbContext _context;

        public PetSpaceController(UserManager<User> userManager, PetSpaceDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            if (model == null) return BadRequest("Data is null");
            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);

                if (model.Role == "Clinic")
                {
                    _context.Clinics.Add(new Clinic { UserId = user.Id, IsVerified = false });
                }
                else if (model.Role == "Vet")
                {
                    _context.Vets.Add(new Vet { UserId = user.Id, IsVerified = false });
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "OK" });
            }

            return BadRequest(result.Errors);
        }
    }

}
