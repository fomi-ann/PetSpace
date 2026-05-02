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
using PetSpace.Core.Domain.Enums;


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
                    .Include(v => v.Clinic)
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
                    
                    isVerified = vet.IsVerified,
                    verifiedAt = vet.VerifiedAt,

                    clinicId = vet.ClinicId,
                    clinicName = vet.Clinic != null ? vet.Clinic.ClinicName : ""
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
                .Where(p =>
                    p.OwnerId == userId ||
                    p.PetOwners.Any(po => po.UserId == userId)
                    )
                .Select(p => new
                {
                    petId = p.PetId,
                    petName = p.PetName,
                    species = p.PetSpecies,
                    breed = p.PetBreed,
                    gender = p.PetGender,
                    birthDate = p.PetBirthDate,
                    microchipNr = p.PetMicrochipNr,
                    weight = p.PetWeight,

                    isPrimaryOwner = p.OwnerId == userId
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

            _context.PetOwners.Add(new PetOwner
            {
                PetOwnerId = Guid.NewGuid(),
                UserId = userId,
                PetId = pet.PetId,
                IsPrimaryOwner = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePetAsync(Guid userId, Guid petId, PetDto dto)
        {
            var pet = await _context.Pets
                .FirstOrDefaultAsync(p => p.PetId == petId && p.OwnerId == userId);

            if (pet == null) return false;

            pet.PetName = dto.PetName;
            pet.PetSpecies = dto.PetSpecies;
            pet.PetBreed = dto.PetBreed;
            pet.PetGender = dto.PetGender;
            pet.PetBirthDate = dto.PetBirthDate;
            pet.PetMicrochipNr = dto.PetMicrochipNr;
            pet.PetWeight = dto.PetWeight;

            pet.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePetAsync(Guid userId, Guid petId)
        {
            var pet = await _context.Pets
                .FirstOrDefaultAsync(p => p.PetId == petId && p.OwnerId == userId);

            if (pet == null) return false;

            var hasAppointments = await _context.Appointments
                .AnyAsync(a => a.PetId == petId);

            var hasMedicalRecords = await _context.MedicalRecords
                .AnyAsync(mr => mr.PetId == petId);

            if (hasAppointments || hasMedicalRecords)
                return false;

            var petOwners = await _context.PetOwners
                .Where(po => po.PetId == petId)
                .ToListAsync();

            _context.PetOwners.RemoveRange(petOwners);

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateAppointmentAsync(Guid userId, CreateAppointmentDto dto)
        {
            //var pet = await _context.Pets
            //    .FirstOrDefaultAsync(p => p.PetId == dto.PetId && p.OwnerId == userId);

            //if (pet == null) return false;

            var hasAccess = await UserHasPetAccessAsync(userId, dto.PetId);

            if (!hasAccess) return false;

            var vet = await _context.Vets.FirstOrDefaultAsync(v => v.VetId == dto.VetId);
            if (vet == null) return false;

            var clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.ClinicId == dto.ClinicId);
            if (clinic == null) return false;

            var appointment = new Appointment
            {
                AppId = Guid.NewGuid(),
                AppDateTime = dto.AppDateTime,
                AppReason = dto.AppReason,
                AppStatusCodeId = (int)AppointmentStatusEnum.Pending,
                PetId = dto.PetId,
                VetId = dto.VetId,
                ClinicId = dto.ClinicId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<object>> GetUserAppointmentsAsync(Guid userId)
        {
            return await _context.Appointments
                .Include(a => a.Pet)
                .Include(a => a.Vet)
                    .ThenInclude(v => v.User)
                .Include(a => a.Clinic)
                .Include(a => a.Status)
                //.Where(a => a.Pet != null && a.Pet.OwnerId == userId)
                .Where(a =>
                    a.Pet != null &&
                    (
                        a.Pet.OwnerId == userId ||
                        a.Pet.PetOwners.Any(po => po.UserId == userId)
                    )
)
                .OrderByDescending(a => a.AppDateTime)
                .Select(a => new
                {
                    appId = a.AppId,
                    appDateTime = a.AppDateTime,
                    appReason = a.AppReason,
                    status = a.Status != null ? a.Status.AppStatusCode : "",
                    petName = a.Pet != null ? a.Pet.PetName : "",
                    vetName = a.Vet != null && a.Vet.User != null
                        ? a.Vet.User.UserFirstName + " " + a.Vet.User.UserLastName
                        : "",
                    clinicName = a.Clinic != null ? a.Clinic.ClinicName : ""
                })
                .Cast<object>()
                .ToListAsync();
        }

        public async Task<List<object>> GetVetAppointmentsAsync(Guid userId)
        {
            var vet = await _context.Vets.FirstOrDefaultAsync(v => v.UserId == userId);
            if (vet == null) return new List<object>();

            return await _context.Appointments
                .Include(a => a.Pet)
                .Include(a => a.Clinic)
                .Include(a => a.Status)
                .Where(a => a.VetId == vet.VetId)
                .OrderByDescending(a => a.AppDateTime)
                .Select(a => new
                {
                    appId = a.AppId,
                    appDateTime = a.AppDateTime,
                    appReason = a.AppReason,
                    status = a.Status != null ? a.Status.AppStatusCode : "",
                    petName = a.Pet != null ? a.Pet.PetName : "",
                    clinicName = a.Clinic != null ? a.Clinic.ClinicName : "",
                    petId = a.PetId
                })
                .Cast<object>()
                .ToListAsync();
        }

        public async Task<bool> UpdateAppointmentStatusAsync(Guid appointmentId, int statusCodeId)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppId == appointmentId);
            if (appointment == null) return false;

            appointment.AppStatusCodeId = statusCodeId;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<object>> GetAllVetsAsync()
        {
            return await _context.Vets
                .Include(v => v.User)
                .Include(v => v.Clinic)
                .Select(v => new
                {
                    vetId = v.VetId,
                    vetName = v.User != null

                        ? v.User.UserFirstName + " " + v.User.UserLastName
                        : "Unknown vet",

                    clinicId = v.ClinicId,
                    clinicName = v.Clinic != null ? v.Clinic.ClinicName : "",
                    specialization = v.VetSpecialization
                })
                .Cast<object>()
                .ToListAsync();
        }

        public async Task<List<object>> GetAllClinicsAsync()
        {
            return await _context.Clinics
                .Select(c => new
                {
                    clinicId = c.ClinicId,
                    clinicName = c.ClinicName,
                    clinicAddress = c.ClinicAddress
                })
                .Cast<object>()
                .ToListAsync();
        }


        public async Task<bool> CreateMedicalRecordAsync(Guid vetUserId, CreateMedicalRecordDto dto)
        {
            var vet = await _context.Vets
                .FirstOrDefaultAsync(v => v.UserId == vetUserId);

            if (vet == null) return false;

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppId == dto.AppId);

            if (appointment == null) return false;

            if (appointment.VetId != vet.VetId) return false;


            var record = new MedicalRecord
            {
                MedicalRecordId = Guid.NewGuid(),
                Diagnosis = dto.Diagnosis,
                TreatmentPlan = dto.TreatmentPlan,
                PetWeight = dto.PetWeight,
                Comment = dto.Comment,

                AppId = appointment.AppId,
                PetId = appointment.PetId,
                VetId = vet.VetId,

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.MedicalRecords.Add(record);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<object>> GetPetMedicalRecordsAsync(Guid userId, Guid petId)
        {
            //var pet = await _context.Pets
            //    .FirstOrDefaultAsync(p => p.PetId == petId && p.OwnerId == userId);

            //if (pet == null)
            //    return new List<object>();

            var hasAccess = await UserHasPetAccessAsync(userId, petId);

            if (!hasAccess)
                return new List<object>();

            return await _context.MedicalRecords
                .Include(mr => mr.Appointment)
                    .ThenInclude(a => a.Clinic)
                .Include(mr => mr.Vet)
                    .ThenInclude(v => v.User)
                .Where(mr => mr.PetId == petId)
                .OrderByDescending(mr => mr.CreatedAt)
                .Select(mr => new
                {
                    medicalRecordId = mr.MedicalRecordId,
                    diagnosis = mr.Diagnosis,
                    treatmentPlan = mr.TreatmentPlan,
                    petWeight = mr.PetWeight,
                    comment = mr.Comment,
                    createdAt = mr.CreatedAt,

                    appointmentDate = mr.Appointment != null
                        ? mr.Appointment.AppDateTime
                        : (DateTime?)null,

                    clinicName = mr.Appointment != null && mr.Appointment.Clinic != null
                        ? mr.Appointment.Clinic.ClinicName
                        : "",

                    vetName = mr.Vet != null && mr.Vet.User != null
                        ? mr.Vet.User.UserFirstName + " " + mr.Vet.User.UserLastName
                        : ""
                })
                .Cast<object>()
                .ToListAsync();
        }

        public async Task<bool> SharePetAsync(Guid currentUserId, SharePetDto dto)
        {
            var pet = await _context.Pets
                .FirstOrDefaultAsync(p => p.PetId == dto.PetId && p.OwnerId == currentUserId);

            if (pet == null) return false;

            var invitedUser = await _userManager.FindByEmailAsync(dto.Email);
            if (invitedUser == null) return false;

            var alreadyShared = await _context.PetOwners
                .AnyAsync(po => po.PetId == dto.PetId && po.UserId == invitedUser.Id);

            if (alreadyShared) return false;

            _context.PetOwners.Add(new PetOwner
            {
                PetOwnerId = Guid.NewGuid(),
                PetId = dto.PetId,
                UserId = invitedUser.Id,
                IsPrimaryOwner = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> UserHasPetAccessAsync(Guid userId, Guid petId)
        {
            return await _context.Pets.AnyAsync(p =>
                p.PetId == petId &&
                (
                    p.OwnerId == userId ||
                    p.PetOwners.Any(po => po.UserId == userId)
                )
            );
        }

        public async Task<bool> RequestClinicVerificationAsync(Guid vetUserId, Guid clinicId)
        {
            var vet = await _context.Vets
                .FirstOrDefaultAsync(v => v.UserId == vetUserId);

            if (vet == null) return false;

            var clinicExists = await _context.Clinics
                .AnyAsync(c => c.ClinicId == clinicId);

            if (!clinicExists) return false;

            vet.ClinicId = clinicId;
            vet.IsVerified = false;
            vet.VerifiedAt = null;
            vet.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<object>> GetClinicVetRequestsAsync(Guid clinicUserId)
        {
            var clinic = await _context.Clinics
                .FirstOrDefaultAsync(c => c.UserId == clinicUserId);

            if (clinic == null) return new List<object>();

            return await _context.Vets
                .Include(v => v.User)
                .Where(v => v.ClinicId == clinic.ClinicId && !v.IsVerified)
                .Select(v => new
                {
                    vetId = v.VetId,
                    vetName = v.User != null
                        ? v.User.UserFirstName + " " + v.User.UserLastName
                        : "Unknown vet",
                    email = v.User != null ? v.User.Email : "",
                    specialization = v.VetSpecialization,
                    licence = v.VetLicence
                })
                .Cast<object>()
                .ToListAsync();
        }

        public async Task<bool> UpdateVetVerificationAsync(Guid clinicUserId, Guid vetId, bool approve)
        {
            var clinic = await _context.Clinics
                .FirstOrDefaultAsync(c => c.UserId == clinicUserId);

            if (clinic == null) return false;

            var vet = await _context.Vets
                .FirstOrDefaultAsync(v => v.VetId == vetId && v.ClinicId == clinic.ClinicId);

            if (vet == null) return false;

            if (approve)
            {
                vet.IsVerified = true;
                vet.VerifiedAt = DateTime.UtcNow;
            }
            else
            {
                vet.IsVerified = false;
                vet.VerifiedAt = null;
                vet.ClinicId = null;
            }

            vet.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> GetClinicVerifiedVetsAsync(Guid clinicUserId)
        {
            var clinic = await _context.Clinics
                .FirstOrDefaultAsync(c => c.UserId == clinicUserId);

            if (clinic == null) return new List<object>();

            return await _context.Vets
                .Include(v => v.User)
                .Where(v => v.ClinicId == clinic.ClinicId && v.IsVerified)
                .Select(v => new
                {
                    vetId = v.VetId,
                    vetName = v.User != null
                        ? v.User.UserFirstName + " " + v.User.UserLastName
                        : "Unknown vet",
                    email = v.User != null ? v.User.Email : "",
                    specialization = v.VetSpecialization,
                    licence = v.VetLicence,
                    verifiedAt = v.VerifiedAt
                })
                .Cast<object>()
                .ToListAsync();
        }

    }
}