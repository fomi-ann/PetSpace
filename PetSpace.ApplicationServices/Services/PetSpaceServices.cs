using Microsoft.AspNetCore.Identity;
using PetSpace.Core.Domain;
using PetSpace.Core.Dto;
using PetSpace.Core.ServiceInterface;
using PetSpace.Data;


namespace PetSpace.ApplicationServices.Services
{
    public class PetSpaceServices : IPetSpaceServices
    {
        private readonly PetSpaceDbContext _context;
        private readonly UserManager<User> _userManager;
        public PetSpaceServices
            (
                PetSpaceDbContext context,
                UserManager<User> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterUserDto dto)
        {
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            return await _userManager.CreateAsync(user, dto.Password);
        }

    }
}
