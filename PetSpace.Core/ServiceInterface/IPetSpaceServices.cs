using Microsoft.AspNetCore.Identity;
using PetSpace.Core.Dto;

namespace PetSpace.Core.ServiceInterface
{
    public interface IPetSpaceServices
    {
        Task<IdentityResult> RegisterUserAsync(RegisterUserDto dto);
        Task<object?> LoginAsync(LoginDto dto);
    }

}
