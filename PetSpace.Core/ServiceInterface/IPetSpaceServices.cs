using Microsoft.AspNetCore.Identity;
using PetSpace.Core.Dto;

namespace PetSpace.Core.ServiceInterface
{
    public interface IPetSpaceServices
    {
        Task<IdentityResult> RegisterUserAsync(RegisterUserDto dto);
        
        Task<object?> LoginAsync(LoginDto dto);
        
        Task<object?> GetUserProfileAsync(Guid userId);
        Task<bool> UpdateUserProfileAsync(Guid userId, UserUpdateDto dto);

        Task<List<object>> GetUserPetsAsync(Guid userId);
        Task<bool> AddPetAsync(Guid userId, PetDto dto);
        Task<bool> DeletePetAsync(Guid userId, Guid petId);

        Task<bool> CreateAppointmentAsync(Guid userId, CreateAppointmentDto dto);
        Task<bool> UpdateAppointmentStatusAsync(Guid appointmentId, int statusCodeId);

        Task<List<object>> GetUserAppointmentsAsync(Guid userId);
        Task<List<object>> GetVetAppointmentsAsync(Guid userId);
    }

}
