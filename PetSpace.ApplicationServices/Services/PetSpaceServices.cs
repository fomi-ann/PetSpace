using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetSpace.Core.Domain;
using PetSpace.Core.Dto;
using PetSpace.Core.ServiceInterface;
using PetSpace.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace PetSpace.ApplicationServices.Services
{
    public class PetSpaceServices : IPetSpaceServices
    {
        private readonly PetSpaceDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public PetSpaceServices
            (
                PetSpaceDbContext context,
                UserManager<User> userManager,
                IConfiguration configuration
            )
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterUserDto dto)
        {
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, dto.Role);

                if (dto.Role == "Vet") _context.Vets.Add(new Vet { UserId = user.Id });
                if (dto.Role == "Clinic") _context.Clinics.Add(new Clinic { UserId = user.Id });
                //if (dto.Role == "PetOwner") _context.PetOwners.Add(new PetOwner { UserId = user.Id });

                await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<object?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing in configuration");
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    role = userRoles.FirstOrDefault()
                };
            }

            return null;
        }

        public async Task<object?> GetUserProfileAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "PetOwner";

            var baseData = new
            {
                userFirstName = user.UserFirstName,
                userLastName = user.UserLastName,
                phoneNumber = user.PhoneNumber,
                email = user.Email,
                role
            };

            if (role == "Vet")
            {
                var vet = await _context.Vets
                    .Include(v => v.User)
                    .FirstOrDefaultAsync(v => v.UserId == userId);

                if (vet == null) return baseData;

                return new
                {
                    baseData.userFirstName,
                    baseData.userLastName,
                    baseData.phoneNumber,
                    baseData.email,
                    baseData.role,
                    specialization = vet.VetSpecialization,
                    licence = vet.VetLicence,
                    isVerified = vet.IsVerified
                };
            }

            if (role == "Clinic")
            {
                var clinic = await _context.Clinics
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (clinic == null) return baseData;

                return new
                {
                    baseData.userFirstName,
                    baseData.userLastName,
                    baseData.phoneNumber,
                    baseData.email,
                    baseData.role,
                    clinicName = clinic.ClinicName,
                    address = clinic.ClinicAddress,
                    phone = clinic.ClinicPhone,
                    isVerified = clinic.IsVerified
                };
            }

            return baseData;
        }

        public async Task<bool> UpdateUserProfileAsync(Guid userId, UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;


            user.UserFirstName = dto.FirstName;
            user.UserLastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;

            var userResult = await _userManager.UpdateAsync(user);
            if (!userResult.Succeeded) return false;


            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role == "Vet")
            {
                var vet = await _context.Vets.FirstOrDefaultAsync(v => v.UserId == userId);
                if (vet != null)
                {
                    vet.VetSpecialization = dto.VetSpecialization ?? vet.VetSpecialization;
                    vet.VetLicence = dto.VetLicence ?? vet.VetLicence;
                    vet.UpdatedAt = DateTime.UtcNow;
                }
            }
            else if (role == "Clinic")
            {
                var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.UserId == userId);
                if (clinic != null)
                {
                    clinic.ClinicName = dto.ClinicName ?? clinic.ClinicName;
                    clinic.ClinicAddress = dto.ClinicAddress ?? clinic.ClinicAddress;
                    clinic.ClinicPhone = dto.ClinicPhone ?? clinic.ClinicPhone;

                    clinic.UpdatedAt = DateTime.UtcNow;
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> GetUserPetsAsync(Guid userId)
        {
            return await _context.Pets
                .Where(p => p.OwnerId == userId)
                .Select(p => new
                {
                    petId = p.PetId,
                    petName = p.PetName,
                    species = p.PetSpecies,
                    breed = p.PetBreed,
                    gender = p.PetGender,
                    birthDate = p.PetBirthDate,
                    microchipNr = p.PetMicrochipNr,
                    weight = p.PetWeight
                })
                .Cast<object>()
                .ToListAsync();
        }

        public async Task<bool> AddPetAsync(Guid userId, PetDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var pet = new Pet
            {
                PetId = Guid.NewGuid(),
                PetName = dto.PetName,
                PetSpecies = dto.PetSpecies,
                PetBreed = dto.PetBreed,
                PetGender = dto.PetGender,
                PetBirthDate = dto.PetBirthDate,
                PetMicrochipNr = dto.PetMicrochipNr,
                PetWeight = dto.PetWeight,
                OwnerId = userId
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePetAsync(Guid userId, Guid petId)
        {
            var pet = await _context.Pets
                .FirstOrDefaultAsync(p => p.PetId == petId && p.OwnerId == userId);

            if (pet == null) return false;

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}